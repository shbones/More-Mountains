using RoR2;
using UnityEngine.Networking;
using UnityEngine;

namespace MoreMountains.Interactables
{
    public class MoreMountainShrineBehavior : NetworkBehaviour
    {
        public Transform IconIndicator;
        public PurchaseInteraction PurchaseInteraction;
        public int NumberOfMountainDirectorStacksToAdd;
        public string InteractableLangToken;
        public int maxPurchaseCount = 1;
        public int purchaseCount = 0;
        public bool waitingForRefresh;
        public float refreshTimer = 0f;
        
        public void Start()
        {
            if (NetworkServer.active && Run.instance)
            {
                IconIndicator.gameObject.SetActive(true);
                PurchaseInteraction.SetAvailableTrue();
            }
            PurchaseInteraction.onPurchase.AddListener(ShrineBehaviorAttempt);
        }

        public void FixedUpdate()
        {
            if (this.waitingForRefresh)
            {
                this.refreshTimer -= Time.fixedDeltaTime;
                if (this.refreshTimer <= 0f && this.purchaseCount < this.maxPurchaseCount)
                {
                    SetShrineEnabled(true);
                    this.waitingForRefresh = false;
                }
            }
            IconIndicator.gameObject.SetActive(PurchaseInteraction.available);
        }

        [Server]
        public void AddShrineStack(Interactor interactor)
        {
            if (!NetworkServer.active)
            {
                Debug.LogWarning("[Server] function 'System.Void RoR2.MoreMountainShrineBehavior::AddShrineStack(RoR2.Interactor)' called on client");
                return;
            }
            this.waitingForRefresh = true;
            if (TeleporterInteraction.instance)
            {
                Log.LogDebug("Adding " + NumberOfMountainDirectorStacksToAdd + " mountain stacks.");
                TeleporterInteraction.instance.AddShrineStack();
                Log.LogDebug("ShrineBonusStacks = " + TeleporterInteraction.instance.shrineBonusStacks);
            }
            CharacterBody component = interactor.GetComponent<CharacterBody>();
            Chat.SendBroadcastChat(new Chat.SubjectFormatChatMessage
            {
                subjectAsCharacterBody = component,
                baseToken = "INTERACTABLE_" + InteractableLangToken + "_MESSAGE"
            });
            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ShrineUseEffect"), new EffectData
            {
                origin = base.transform.position,
                rotation = Quaternion.identity,
                scale = 1f,
                color = new Color(0.7372549f, 0.90588236f, 0.94509804f)
            }, true);
            this.refreshTimer = 2f;
            BossDropManager.Instance.AddMoreMountainReward(this.gameObject.name);
            ShrineActivationTracker.Instance.TrackVariantMountainShrineHit(this.gameObject.name);
            SetShrineEnabled(false);
        }

        public void ShrineBehaviorAttempt(Interactor interactor)
        {
            ++purchaseCount;
            AddShrineStack(interactor);
        }

        public void SetShrineEnabled(bool activeState)
        {
            PurchaseInteraction.SetAvailable(activeState);
            IconIndicator.gameObject.SetActive(activeState);
        }
    }
}
