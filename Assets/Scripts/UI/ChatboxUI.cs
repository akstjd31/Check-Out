using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ChatboxUI : MonoBehaviour
{
    //대화 창 내에서 말하는 주체의 이름이 출력될 텍스트 공간.
    [Header("이름 출력 공간")]
    [SerializeField] TextMeshProUGUI nameText;

    //대화 창 내에서 대사가 출력될 텍스트 공간.
    [Header("대사 출력 공간")]
    [SerializeField] TextMeshProUGUI descriptionText;

    //한 글자씩 출력하는 데 걸리는 시간.
    [Header("대사 출력 딜레이")]
    [SerializeField] float charPrintDelay = 0.004f;

    //캔버스 내에서, 대화 창을 담당하는 칸의 최상위 부모 오브젝트.
    [Header("대화 창")]
    [SerializeField] CanvasGroup chatboxGroup;

    [Header("CG 출력용 이미지 칸")]
    [SerializeField] Image spriteCG;

    //대사 출력 정보를 받아올 테이블 ( 미구현 상태이므로 관련 오류는 무시하고 작성 )
    Table<int, TalkTableData> talkTable;

    //코루틴 동작을 담기 위한 코루틴 매개변수
    private Coroutine typing;
    
    //대사가 출력 중인지 결정하기 위한 코드. (기능 구현이 성공적으로 이루어졌을 때 추가적으로 작성)
    private bool isTyping;

    //출력할 대사를 담아줄 문자열형 매개변수
    private string description = " ";

    public int id { get; private set; }
    public int next_id { get; private set; }
    public string namae { get; private set; }
    public string line_desc { get; private set; }
    public string CG { get; private set; }
    public float delay { get; private set; }

    public bool IsTyping => isTyping;
    public string Script => description;


    private void Start()
    {
        //테이블매니저로부터 대사 출력 정보를 담은 테이블을 받아온다.
        talkTable = TableManager.Instance.GetTable<int, TalkTableData>();

        Debug.Log("데이터 받아오는 과정 거쳤음");
        //없을 경우 안내 후 반환.
        if (talkTable == null)
        {
            Debug.Log("대화 생성 테이블이 null입니다.");
            return;
        }

        //테이블매니저로부터 테이블 내 각각의 ID를 불러와서
        foreach (int targetId in TableManager.Instance.GetAllIds(talkTable))
        {
            //대사 테이블 데이터의 대상 아이디 데이터를 대입.
            TalkTableData talkTableData = talkTable[targetId];
        }
    }

    private void OnEnable()
    {
        if (!ChatboxManager.Instance.isUsingChatbox) Hide();
    }

    /// <summary>
    /// 대화창을 띄우는 메서드입니다.
    /// </summary>
    public void Show()
    {
        chatboxGroup.alpha = 1f;
        chatboxGroup.interactable = true;
        chatboxGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// 대화창을 숨기는 메서드입니다.
    /// </summary>
    public void Hide()
    {
        chatboxGroup.alpha = 0f;
        chatboxGroup.blocksRaycasts = false;
        chatboxGroup.interactable = false;
    }

    /// <summary>
    /// 아이디 값을 입력받았을 때, 해당 아이디 값에 맞는 대사를 출력하는 메서드.
    /// </summary>
    /// <param name="desc_id">대사의 id값.</param>
    public void Description(int desc_id)
    {
        //다음 대사가 없는 것을 거쳐 해당 과정을 거쳤을 경우 종료하기 위한 대비코드.
        if(desc_id == 0)
        {
            Hide();
            return;
        }
        
        
        //이름과 대사를 테이블 내의 해당 아이디값에서 불러온다.
        nameText.text = talkTable[desc_id].name;
        description = talkTable[desc_id].line_desc;

        //테이블에서 CG가 있는 id값이었을 경우
        if (talkTable[desc_id].CG != null)
        {
            //CG창 활성화
            spriteCG.gameObject.SetActive(true);
            //CG창의 스프라이트를 Resource 폴더 내 경로에서 받아와 대입
            spriteCG.sprite = Resources.Load<Sprite>(talkTable[desc_id].CG);
        }

        //그 외, 테이블에서 CG가 없는 id값이었을 경우 (안전을 위해 null 및 비어있는 칸으로 입력)
        else if (talkTable[desc_id].CG == null || talkTable[desc_id].CG == "")
        {
            //CG창 스프라이트를 제거
            spriteCG.sprite = null;
            //CG창 비활성화
            spriteCG.gameObject.SetActive(false);
        }
        
        //아이디값으로부터 찾아온 대사를 출력 시작한다.
        StartTyping(description);
    }

    /// <summary>
    /// 아이디 값을 입력받아, 해당 값의 다음 id값을 반환합니다.
    /// </summary>
    /// <param name="desc_id"></param>
    /// <returns></returns>
    public int DescriptionInput(int desc_id)
    {
        //대사 테이블에서 해당 아이디의 next_id 값이 존재하지 않을 경우, 0을 반환한다.
        if (talkTable[desc_id].next_id == 0)
        {
            Hide();
            return 0;
        }

        //존재하는 경우, 그 next_id 값을 반환한다.
        return talkTable[desc_id].next_id;
    }

    /// <summary>
    /// 문자열을 입력받으면 내부의 한 글자씩을 출력하는 메서드입니다.
    /// </summary>
    /// <param name="Text"></param>
    public void StartTyping(string Text)
    {
        //이미 동작 중이었다면 해당 동작을 멈춘다
        if(typing != null)
            StopCoroutine(typing);

        //동작에 입력받은 텍스트를 출력하는 코루틴 시작을 집어넣는다.
        typing = StartCoroutine(StringPrinter(Text));
    }

    /// <summary>
    /// 한 글자씩 출력하는 코루틴입니다.
    /// </summary>
    /// <param name="description">출력할 문자열</param>
    /// <returns></returns>
    public IEnumerator StringPrinter(string description)
    {
        //출력 중!
        isTyping = true;
        //아무것도 출력하지 않은 상태일 테니 초기화. 텅 빈 상태로 만들기.
        descriptionText.text = " ";

        //출력할 문자열의 각 문자 하나하나마다
        foreach(var c in description)
        {
            //출력용 텍스트 칸에 하나씩 담고
            descriptionText.text += c;
            //출력용 대기시간만큼 대기
            yield return new WaitForSeconds(charPrintDelay);
        }

        //여기까지 왔다면 끝난 것이니 출력 끝남 표시
        isTyping= false;
        //코루틴 비우기
        typing = null;
    }

    /// <summary>
    /// 입력 중인 상태에서 해당 문자열을 즉시 출력하는 메서드입니다.
    /// </summary>
    /// <param name="Text">출력할 문자열</param>
    public void SkipTyping(string Text)
    {
        //출력 중이 아니라면 그냥 반환합니다.
        if (!isTyping)
            return;

        //이미 입력 중이었을 경우 해당 입력을 중지합니다.
        if (typing != null)
            StopCoroutine(typing);

        //대사창의 텍스트는 입력받은 문자열로 즉시 바꿉니다.
        descriptionText.text = Text;
        //입력 끝.
        isTyping = false;
        //코루틴 초기화.
        typing = null;
    }    
}
