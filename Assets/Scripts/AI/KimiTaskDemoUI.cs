using TMPro;
using UnityEngine;

namespace AI
{
    public class KimiTaskDemoUI : MonoBehaviour
    {
        [SerializeField] private KimiTaskClient client;
        [SerializeField] private TMP_Text taskText;

        [TextArea(2, 6)]
        [SerializeField]
        private string overrideUserPrompt =
            "给我五个数字，不要生成别的任何内容。";

        private void Start()
        {
            RequestOne();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
                RequestOne();
        }

        public void RequestOne()
        {
            if (client == null || taskText == null) return;

            taskText.text = "updating...";

            client.GenerateTask(
                overrideUserPrompt,
                onSuccess: (t) => taskText.text = t,
                onError: (e) => taskText.text = "Fail：\n" + e
            );
        }
    }
}
