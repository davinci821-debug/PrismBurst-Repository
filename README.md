# Prism Burst

턴제 RPG + 타일 퍼즐 + 포물선 슈팅 시스템을 결합한  
C# 콘솔 기반 게임 프로젝트입니다.

Prism Burst는 백야극광의 타일 시스템과 포트리스의 포물선 슈팅 시스템에서 영감을 받아 제작한 학습형 프로젝트입니다.  
객체지향 프로그래밍, FSM(State Machine), 콘솔 UI 렌더링, 상태이상 시스템 등을 직접 구현하며 게임 구조 설계를 학습하는 것을 목표로 제작했습니다.

---

# 프로젝트 소개

Prism Burst는 색상 타일을 연결하여 강화된 마법을 사용하는 콘솔 기반 턴제 RPG 게임입니다.

플레이어는 “컬러매지션”이 되어:
- 속성을 선택하고
- 같은 색 타일을 이동하며 콤보를 만들고
- 각도와 파워를 조절해 마법을 발사합니다.

이 프로젝트는:
- FSM(State Machine)
- 턴제 전투 시스템
- 타일 퍼즐
- 포물선 물리
- 객체지향 구조

등을 학습하기 위해 제작되었습니다.

---

# 사용 기술

## 개발 환경
* C#
* Visual Studio 2022

---

## 객체지향 프로그래밍 요소
* 클래스(Class)
* 상속(Inheritance)
* 추상 클래스(Abstract Class)
* 프로퍼티(Property)
* 캡슐화(Encapsulation)
* 가상 메서드(Virtual Method)

---

## 사용 자료구조
* List<T>
* Stack<T>
* 2차원 배열
* Enum Flags
* Struct

---

# 구현 기능

## 1. FSM 기반 턴제 전투 시스템

게임은 상태(State)에 따라 행동이 변경되는 FSM 구조로 제작했습니다.

```text
SelectMagic
→ MoveTile
→ Aim
→ MonsterTurn
→ 반복
```

현재 상태에 따라:
- 속성 선택
- 타일 이동
- 조준
- 공격
- 몬스터 턴

이 구분되어 진행됩니다.

---

## 2. 타일 콤보 시스템

플레이어는 같은 색 타일을 연속 이동하여 콤보를 누적할 수 있습니다.

```csharp
Stack<TileType> moveTiles;
```

최근 이동한 타일부터 검사하여 콤보를 계산합니다.

---

## 속성별 특징

| 색상 | 효과 |
|---|---|
| 빨강 | 공격력 증가 |
| 파랑 | 보호막 증가 |
| 노랑 | 스턴 확률 증가 |
| 초록 | 중독 상태 부여 |

---

## 3. 포물선 탄도 시스템

BulletManager에서:
- 포물선 이동
- 중력
- 충돌 판정
- 각도 계산
- 파워 게이지

를 구현했습니다.

```csharp
velocityY += gravity;
```

중력 기반 탄도 시스템을 사용합니다.

---

## 4. 상태이상 시스템

Flags Enum 기반 상태이상 시스템을 구현했습니다.

### 현재 구현 상태이상
* 스턴
* 중독
* 보호막 증가
* 크리티컬 공격

---

## 5. 몬스터 AI 및 위험도 시스템

몬스터는 플레이어와 가까워질수록:

- 공격력 증가
- 크리티컬 증가
- 위험도 상승

상태가 변경됩니다.

### 위험도 단계
* Warning
* Critical
* Berserk

---

## 6. 콘솔 UI 시스템

Console.SetCursorPosition() 기반 좌표형 UI를 구현했습니다.

### 현재 구현 UI
* HP 게이지
* 전투 로그
* 타일맵
* 속성 UI
* 전투 상태 UI
* 몬스터 상태 UI

---

# 프로젝트 구조

```text
Program
 └─ TitleScene
 └─ BattleManager
      ├─ TurnSystem
      ├─ BulletManager
      ├─ TileManager
      ├─ DamageManager
      ├─ BattleLog
      ├─ Player
      ├─ Monster
      ├─ MonsterManager
      ├─ Character
      ├─ GameScene
      ├─ Enum
      └─ Struct(Position)
```

---

# 추후 구현 예정 기능

초기 기획에서는 다음 기능들을 목표로 했습니다.

* 상점 시스템
* 카지노 시스템
* 인벤토리
* 저장 시스템
* 마을 맵
* 다양한 전투 맵
* 장비 시스템
* 스킬 확장
* 스토리 시스템
* 보스 패턴 시스템

현재는 핵심 전투 시스템 구현에 우선 집중했습니다.

---

# 프로젝트를 통해 배운 점

이 프로젝트를 제작하며:

- 객체지향 구조 설계
- 클래스 역할 분리
- FSM(State Machine)
- 게임 루프 구조
- 콘솔 UI 렌더링
- 포물선 물리 계산
- 상태 기반 전투 시스템

등을 학습할 수 있었습니다.

처음에는 대부분의 기능을 BattleManager에 구현했지만  
코드가 복잡해지고 유지보수가 어려워져:

- TurnSystem
- BulletManager
- TileManager
- DamageManager

등으로 역할을 분리하며 객체지향 설계의 중요성을 배울 수 있었습니다.
