using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSpawnLocation", menuName = "Scriptable Objects/MonsterSpawnLocation")]
public class MonsterSpawnLocation : ScriptableObject
{
    public int id;

    //프리팹을 가져오는 건 대략 이런 방식이다. 프리팹 파일의 이름까지 작성.
    //GameObject Prefab = Resources.Load<GameObject>("Prefabs/Monster/MonsterExample");

    public GameObject SpawnMonsterChecker()
    {
        //아이디에 맞는 몬스터의 경로를 찾아가서
        GameObject monster = Resources.Load<GameObject>($"");

        //해당 몬스터를 반환한다.
        return monster;
    }
}
