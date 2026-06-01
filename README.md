# 👻 Check-Out

<a href="https://youtu.be/ttOinEBZzFM">
    <img width="1192" height="608"
         alt="image"
         src="https://github.com/user-attachments/assets/620650af-0755-4038-9e55-5aebbd5ddc5e" />
</a>

> 기획 파트와 협업하여 제작한 3D 1인칭 호러 익스트랙션 게임


> 📺 이미지를 클릭하면 플레이 영상을 확인할 수 있습니다.

---

# 📑 목차

- [📌 프로젝트 개요](#-프로젝트-개요)
- [🎮 게임 소개](#-게임-소개)
- [⚙ 주요 시스템](#-주요-시스템)
- [🛠 기술적 구현](#-기술적-구현)
- [🤔 기술적 고민 및 해결](#-기술적-고민-및-해결)
- [🤝 협업 경험](#-협업-경험)
- [📈 성과](#-성과)
- [💡 회고](#-회고)

---

# 📌 프로젝트 개요

| 항목    | 내용                      |
| ----- | ----------------------- |
| 프로젝트명 | Check-Out               |
| 개발 기간 | 2025.12.11 ~ 2025.12.31 |
| 개발 인원 | 11명 (개발 5, 기획 6)                    |
| 담당 역할 | 클라이언트 개발                |
| 개발 환경 | Unity 6000.2.10f1       |
| 플랫폼   | PC                      |
| 장르    | 3D 1인칭 호러 익스트랙션         |

---

# 🎮 게임 소개

플레이어는 제한된 공간에서 위험 요소를 회피하며

목표 아이템을 수집하고 탈출해야 합니다.

탐색 과정에서 다양한 이벤트가 발생하며,

플레이어는 제한된 자원과 정보를 활용하여 생존해야 합니다.

---

# ⚙ 주요 시스템

## 게임 흐름 시스템

* Hub → Session → Result 구조
* 상태 기반 게임 진행
* 씬 전환 관리

<img width="600" height="300" alt="ezgif com-video-to-gif-converter" src="https://github.com/user-attachments/assets/2c6d1d47-a62b-4101-864c-e8c83d9c1505" />

---

## 이벤트 트리거 시스템

* 특정 오브젝트 진입 시 이벤트 발생
* 사운드, 대사, 상호작용 등 다양한 이벤트 지원
* 이벤트 활성화 및 비활성화 관리
* 게임 진행도에 따른 이벤트 제어

<img width="600" height="300" alt="download-ezgif com-video-to-gif-converter" src="https://github.com/user-attachments/assets/62951982-806b-4beb-8a9c-80195d3e7f7e" />

<img width="600" height="300" alt="ezgif com-video-to-gif-converter" src="https://github.com/user-attachments/assets/e90bb22b-0009-4959-abec-1aa6d08f8914" />

---

## 저장 시스템

* 진행 상태 저장
* 아이템 정보 저장
* 이벤트 진행 상태 저장
* 게임 재실행 시 데이터 복구

<img width="600" height="300" alt="ezgif com-video-to-gif-converter (1)" src="https://github.com/user-attachments/assets/213a655f-ea30-4278-a43b-d5edb29c20da" />

---

# 🛠 기술적 구현

## 1. State 패턴 기반 씬 전환 시스템

### 문제

게임 흐름이 복잡해지면서

씬 전환 로직이 여러 곳에 분산될 가능성이 있었습니다.

### 해결

State 패턴을 활용하여

* Init
* Main
* Hub
* Session
* Dead

상태를 독립적으로 관리했습니다.

### 결과

* 게임 흐름 구조 명확화
* 씬 전환 책임 분리
* 유지보수성 향상

<img width="1200" height="200" alt="image" src="https://github.com/user-attachments/assets/65739cf2-219d-4ea4-88ac-76b2501e7e53" />

---

## 2. 이벤트 트리거 시스템

### 문제

사운드 재생, 대사 출력, 상호작용 활성화 등의 이벤트를

개별 오브젝트마다 구현할 경우

* 이벤트 로직 중복 발생
* 신규 이벤트 추가 비용 증가
* 테스트 및 유지보수 어려움 발생

### 해결

맵에 이벤트 트리거를 배치하고,

이벤트 조건과 실행 로직을 분리하여

공통된 방식으로 이벤트를 관리할 수 있는 구조를 설계했습니다.

이를 통해 사운드, 대사, 상호작용 활성화 등

다양한 이벤트를 동일한 흐름으로 처리할 수 있도록 구현했습니다.

### 결과

* 신규 이벤트 추가 용이
* 이벤트 관리 구조 단순화
* 기획 파트 테스트 효율 향상
* 유지보수성 향상

---

## 3. Generic Save / Load 시스템

### 문제

저장해야 하는 데이터 종류가 증가하면서

저장 구조가 복잡해질 수 있었습니다.

### 해결

제네릭 기반 Save/Load 구조를 설계하여

다양한 데이터 타입을 동일한 인터페이스로 저장할 수 있도록 구현했습니다.

### 결과

* 저장 로직 재사용 가능
* 데이터 확장 용이
* 코드 중복 감소

```csharp
public void Save<T>(string fileName, T data) where T : SaveBase
{
    string path = GetPath(fileName);
    string json = JsonUtility.ToJson(data, true);
    File.WriteAllText(path, json);
}

public T Load<T>(string fileName) where T : SaveBase, new()
{
    string path = GetPath(fileName);

    if (!File.Exists(path))
        return null;

    string json = File.ReadAllText(path);
    return JsonUtility.FromJson<T>(json);
}
```

# 🤔 기술적 고민 및 해결

## 왜 State 패턴을 사용했는가?

게임 흐름을 상태 단위로 분리하여

씬 전환 책임을 명확하게 관리하기 위해 적용했습니다.

---

## 왜 이벤트 트리거 시스템을 설계했는가?

게임 진행에 따라

- 사운드 재생
- 대사 출력
- 상호작용 활성화

등 다양한 이벤트가 필요했습니다.

이벤트마다 서로 다른 구현 방식을 사용할 경우
관리 비용이 증가할 수 있었기 때문에,

공통된 방식으로 이벤트를 처리할 수 있는
이벤트 트리거 시스템을 설계했습니다.

---

## 왜 Generic Save/Load 구조를 사용했는가?

저장 대상이 늘어나더라도

기존 저장 로직을 수정하지 않고 확장할 수 있도록 설계했습니다.

---

# 🤝 협업 경험

## 기획 파트와의 협업

기획자가 이벤트를 직접 검증할 수 있도록

프로토타입 기반 테스트 환경을 구축했습니다.

이를 통해

* 이벤트 테스트 비용 감소
* 요구사항 전달 효율 향상
* 피드백 반영 속도 개선

효과를 얻을 수 있었습니다.

---

# 📈 성과

* State 패턴 기반 게임 흐름 설계
* 이벤트 트리거 시스템 구현
* Generic Save / Load 시스템 구현
* 데이터 기반 이벤트 구조 설계
* 기획 파트와의 협업 경험

---

# 💡 회고

이번 프로젝트에서는

단순 기능 구현보다

* 게임 흐름 설계
* 이벤트 구조 설계
* 협업 프로세스 개선

에 집중했습니다.

특히 기획 파트와 지속적으로 피드백을 주고받으며

'개발자가 구현하기 쉬운 구조' 뿐만 아니라

'기획자가 테스트하기 쉬운 구조' 역시 중요하다는 점을 경험할 수 있었습니다.
