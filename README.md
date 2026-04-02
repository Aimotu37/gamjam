上次更新：

---
## 一、项目背景

**游戏类型：** RPG 点击解密像素风横版探索
**引擎：** Unity（C#）
**团队：** 🐟鱼文学🐟

### Demo 场景流程
```
Script1（主菜单）→ Script2（开场）→ Script3（成年卧室）
→ Script4（梦境1：童年卧室）→ Script5（醒来+密码输入）
→ Script6（梦境2卧室）→ Script6_1（街道）→ Script7（现实）
```

---
## 二、代码架构
待更新（未完整）2025/04/03

### 三层结构
```
CommonScripts/（公共层）
  temporary:功能调试代码，可以不看
  SceneManagerBase.cs   ← 所有 GameManager 基类
  IGameManager.cs       ← 接口
  GlobalData.cs         ← 跨场景进度（密码线索+日记解锁）
  GlobalEnums.cs        ← RoomState房间状态机 / ItemType物品种类 / DiaryID日记页 / PortraitOption立绘表情
  DialogueManager.cs    ← 对话系统
  PopupSystem.cs        ← 物件简介弹窗
  InteractableItem.cs   ← 可交互物件基类
  NotebookUI.cs         ← 日记本UI（多场景共用）
  NoteUI.cs             ← 便利贴UI
  StateAction.cs        ← Action基类
  ActionSequenceTrigger.cs
  PlayDialogueAction.cs
  PlayClipAction.cs
  ShowItemInfoAction.cs ← 触发弹窗
  CollectItemAction.cs  ← 写入GlobalData进度

Script4/
  GameManager.cs        ← 继承SceneManagerBase
  Task_S4.cs
  S4StateController.cs  ← 调试快捷键1~5

Script6/
  GameManager.cs
  Task_S6.cs

Script6_1/
  GameManager.cs
  Task_S61.cs
  SnackManager.cs
```

### 事件流
```
点击物件
→ InteractableItem 执行 defaultActions
→ ShowItemInfoAction  弹出简介（PopupSystem）
→ CollectItemAction   写入 GlobalData
→ GlobalData.OnDiaryUnlocked 事件
→ Task HUD 刷新 / NotebookUI.RefreshDiaryPages()
```
---
## 三、已完成文件（可直接用）
- 待更新
---
## 四、Unity Inspector 已完成配置
- S4基本交互内容
---
## 五、当前卡点、
** 鱼缸点击后位置与简介重合会触发关闭，导致闪避**
- 要限制关闭按钮范围
---
## 六、接下来的目标（按顺序）
- 待更新
---
## 七、注意事项

- Unity 文件名必须和类名一致（GameManager.cs 不能叫 S4_GameManager.cs）
- `FindAnyObjectByType<SceneManagerBase>()` 是所有脚本获取 GameManager 的统一方式



