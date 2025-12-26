using UnityEngine;
using UnityEngine.SceneManagement;

public class ChatboxManager : Singleton<ChatboxManager>
{
    [Header("대화창 UI")]
    public ChatboxUI chatboxUI;

    //외부 오브젝트 등으로부터 받아올 현재 id값
    public int eventId = 92001;

    public bool isUsingChatbox = false;

    private ChatboxUI chatbox;

    private void Start()
    {
        chatbox = Instantiate(chatboxUI);
        chatbox.transform.parent = ChatboxManager.Instance.transform;
        EndDialogue();
    }


    void Update()
    {
        if(chatboxUI == null) GetComponent(typeof(ChatboxUI));
        //상호작용 키는 E입니다.
        if (Input.GetKeyDown(KeyCode.E))
        {
            //대사 출력 중인 상태라면
            if (chatbox.IsTyping)
            {
                //대사 출력을 생략
                chatbox.SkipTyping(chatbox.Script);
            }
            //대사 출력 중이 아니라면
            else
            {
                //이벤트 ID가 0이 아닌 경우에
                if (eventId != 0)
                {
                    //이벤트 ID 기준 다음 대사 ID가 존재하는지 확인하고
                    eventId = chatbox.DescriptionInput(eventId);
                    //그 값을 기준으로 다시 한번 사용
                    chatbox.Description(eventId);
                }
                //만에 하나를 대비하여 0일 때의 코드 작성
                else if (eventId == 0)
                    EndDialogue();
            }
        }
    }

    /// <summary>
    /// 대사 출력 시 실행할 메서드입니다.
    /// </summary>
    public void StartDialogue()
    {
        if(!isUsingChatbox)
            isUsingChatbox = true;
        //대사창 활성화하기
        chatbox.Show();
        //대사 ID값을 넣어 대사 출력 실행하기
        chatbox.Description(eventId);
    }

    /// <summary>
    /// 대사 종료 시 실행할 메서드입니다.
    /// </summary>
    public void EndDialogue()
    {
        //대사창 비활성화하기
        chatboxUI.Hide();
        if(isUsingChatbox)
            isUsingChatbox = false;
    }

    public void StartChatbox(int id)
    {
        Debug.Log($"{id}");
        eventId = id;
        StartDialogue();
    }

}
