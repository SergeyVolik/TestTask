using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
namespace TestTaskProject
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text m_Text;

        public bool TextEnabled => m_Text.enabled;
        // Start is called before the first frame update
        void Awake()
        {
            m_Text.enabled = false;
            MessageBroker.Default.Receive<MessageShowIteractUI>().Subscribe((msg) =>
            {
                Debug.Log("showUI");
                m_Text.enabled = true;
            });

            MessageBroker.Default.Receive<MessageHideIteractUI>().Subscribe((msg) =>
            {
                Debug.Log("hideUI");
                m_Text.enabled = false;
            });
        }

    }
}
