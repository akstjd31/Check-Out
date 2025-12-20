// 상태 인터페이스
public interface IState
{
    void Enter();
    void Update();
    void Exit();
}

// 게임의 전체적인 상태
public enum GameState
{
    Main,       // 메인 화면
    Hub,        // 휴식 공간
    Loading,    // 로딩 중
    Session     // 세션 입장
}