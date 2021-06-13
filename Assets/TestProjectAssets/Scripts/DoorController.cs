using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

namespace TestTaskProject
{
    public class DoorController : MonoBehaviour
    {
        private ThirdPersonMovement m_Player;
        private AudioSource m_Sound;
        [SerializeField]
        private Transform m_Door;

        [SerializeField]
        float moveXDistance = 1f;

        private bool m_playerInside = false;
       
        private float m_StartX;
        private bool m_AnimationPlaying;
        private bool m_UiShowed;
        private bool m_DoorOpened;


        private void Awake()
        {
            m_Sound = GetComponent<AudioSource>();
            m_Player = FindObjectOfType<ThirdPersonMovement>();
            m_StartX = m_Door.transform.localPosition.x;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constans.PLAYER_TAG))
            {
                m_playerInside = true;
               
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Constans.PLAYER_TAG))
            {
                m_playerInside = false;
            }
        }


        private void Update()
        {
            if (m_playerInside && !m_AnimationPlaying)
            {
                    
                    var pos1 = Vector3.ProjectOnPlane(transform.position, Vector3.up);
                    var pos2 = Vector3.ProjectOnPlane(m_Player.transform.position, Vector3.up);

                    Debug.Log(Vector3.Dot(m_Player.transform.forward, (pos1 - pos2).normalized));
                    float dotValue = Vector3.Dot(m_Player.transform.forward, (pos1 - pos2).normalized);
                    bool playerLookAtDoorPanel = dotValue > 0.5f ? true : false;

                    if (playerLookAtDoorPanel)
                    {

                        if (!m_UiShowed)
                        {
                            MessageBroker.Default.Publish(new MessageShowIteractUI());
                            m_UiShowed = true;
                        }

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            if (m_DoorOpened)
                            {
                                CloseDoor();
                            }
                            else
                            {
                                OpenDoor();

                            }

                        }

                    }
                else
                {
                    MessageBroker.Default.Publish(new MessageHideIteractUI());
                    m_UiShowed = false;
                }

            }

            else if(m_UiShowed)
            {

                MessageBroker.Default.Publish(new MessageHideIteractUI());
                m_UiShowed = false;
                
            }
        }

        private void OpenDoor()
        {
            m_Sound.Play();
            m_UiShowed = false;
            m_DoorOpened = true;
            m_AnimationPlaying = true;
            MessageBroker.Default.Publish(new MessageHideIteractUI());
            MessageBroker.Default.Publish(new MessageInteractAction());
            m_Door.DOLocalMoveX(m_StartX + moveXDistance, 1f).SetEase(Ease.Linear).OnComplete(() => m_AnimationPlaying = false).Play();
        }
        private void CloseDoor()
        {
            m_Sound.Play();
            m_UiShowed = false;
            MessageBroker.Default.Publish(new MessageHideIteractUI());
            MessageBroker.Default.Publish(new MessageInteractAction());
            m_DoorOpened = false;
            m_Door.DOLocalMoveX(m_StartX, 1f).SetEase(Ease.Linear).OnComplete(() => m_AnimationPlaying = false).Play();
            m_AnimationPlaying = true;
        }

    }
}