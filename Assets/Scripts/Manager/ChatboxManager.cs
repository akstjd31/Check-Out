using UnityEngine;

public class ChatboxManager : MonoBehaviour
{
    [Header("대화창 UI")]
    [SerializeField] ChatboxUI chatboxUI;

    //외부 오브젝트 등으로부터 받아올 현재 id값
    public int eventId = 92001;

    private void Start()
    {
        EndDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (chatboxUI.IsTyping)
            {
                chatboxUI.SkipTyping(chatboxUI.Script);
            }
            else
            {
                eventId = chatboxUI.DescriptionInput(eventId);
                chatboxUI.Description(eventId);
            }
        }
    }
    public void StartDialogue()
    {
        chatboxUI.Show();
        chatboxUI.Description(eventId);
    }

    public void EndDialogue()
    {
        chatboxUI.Hide();
    }
}
