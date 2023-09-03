using System.Linq;
using Character;
using Core.Constants;
using Core.DataPersistence;
using Core.DataPersistence.Data;
using Infrastructure.InputSystem;
using Infrastructure.Attributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Zenject;
using System.Collections;
using LDtkUnity;

namespace Collecting
{
    public class Chest : MonoBehaviour, IDataPersistence
    {
        public string ID { get; private set; }
        public Transform InteractDisplayTransform;
        public GameObject ParticleTransform; 
        public Animator Animator;
        public UnityEvent OnOpen;

        private bool IsOpened = false;
        private PlayerHandler PlayerHandler;

        #region Dependencies
        [Inject]
        private PlayerInputControl playerInputControl;
        #endregion

        private void Awake() 
        {
            ID = GetComponent<LDtkIid>()?.Iid;
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            PlayerHandler = other.GetComponent<PlayerHandler>();
            if(PlayerHandler != null)
            {
                InteractDisplayTransform.gameObject.SetActive(IsOpened == false);
                PlayerHandler.InteractAction = Open;
            }
        }

        private void OnTriggerExit2D(Collider2D other) 
        {
            PlayerHandler = other.GetComponent<PlayerHandler>();
            if(PlayerHandler != null)
            {
                InteractDisplayTransform.gameObject.SetActive(false);
                PlayerHandler.InteractAction = null;
            }
        }

        private void SetOpenedGUI()
        {
            IsOpened = true;
            InteractDisplayTransform.gameObject.SetActive(false);
            ParticleTransform.SetActive(true);
            Animator.SetTrigger(AnimationParameter.Open);
        }

        private void Open()
        {
            if(IsOpened == false)
            {
                SetOpenedGUI();
                OnOpen?.Invoke();
                PlayerHandler?.AddOpenedChestID(ID);
            }
        }

        private void Close()
        {
            Animator.SetTrigger(AnimationParameter.Close);
        }

        public void LoadData(GameDataModel data)
        {
            if(data.OpenedChestIDs.Contains(ID))
            {
                SetOpenedGUI();
            }
        }

        public void SaveData(GameDataModel data)
        {
        }
    }
}
