using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UIUtils : MonoBehaviour
    {
        [SerializeField]
        GameObject ErrorField = null;
        [SerializeField]
        GameObject LoadingBar = null;
        public static UIUtils instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.Log("Instance already exsists, destroying...");
                Destroy(this);
            }

            LoadingBar.SetActive(false);
        }

        public void DisplayError(string _error)
        {
            ErrorField.GetComponent<Text>().text = _error;
            StartCoroutine("ErrorPopup");
        }

        IEnumerator ErrorPopup()
        {
            ErrorField.SetActive(true);
            yield return new WaitForSeconds(3f);
            ErrorField.SetActive(false);
        }

        public void SetLoading(bool flag)
        {
            LoadingBar.SetActive(flag);
        }
    }
}
