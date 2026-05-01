using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RoomState 按场景分组加注释

//  场景状态
public enum RoomState
{
    None,

    // ── Script4：梦境1 卧室 ──
    Intro,               // 开场自动对白
    NoteLocked,          // 可以交互，密码本锁定
    PasswordCollecting,  // 3 个线索都找到，可以输入密码
    AllTasksDone,        // 密码本解锁，日记1解锁
    ReadyToExit,         // 准备醒来

    // ── Script6：梦境2 卧室→街道 ──
    S6_Bedroom_Intro,    // 卧室开场
    Dream2_Bedroom,      // 卧室可交互
    ReadyToStreet,       // 准备出门
    Dream2_Street,       // 街道主场景
    Dream2_TaskCompleted,// 街道任务全完成
    ReadyToS7,           // 准备进入 Script7

    // ── 预留：后续 Script 加在这里 ──
    // S3_Bedroom_Intro,

    // ── Script5：成年卧室 ──
    S5_Intro,
    S5_NoteBookReadFinish,
    S5_InteractionDone,


    // S7_Reality,
}

//  可交互物件类型
public enum ItemType
{
    // Script4 物件
    Bed,        // 床
    NoteBook,   // 密码本
    Note,       // 便利贴
    FishTank,   // 鱼缸
    Doll,       // 玩偶
    Award,      // 奖状
    Beads,      // 串珠
    Duck,       // 橡皮鸭

    // Script6 物件
    Map,        // 手绘地图
    Cart,       // 小吃车
    TV,         // 电视（百货大楼）
    GachaMachine,   // 扭蛋机
    ToyPhone,       // 玩具电话
    Computer,       // 网吧电脑
    RockingCar,     // 摇摇车
}

public enum DiaryID
{
    None,
    Diary1_FishAndBeads,      // 热带鱼和串珠
    Diary2_SnackCart,         // 小吃车
    Diary3_SummerTV,          // 暑假的电视节目
    Diary4_SnackGachaToyPhone,// 零食扭蛋玩具电话
    Diary5_Stationery,        // 文具店的笔
    Diary6_BooksAndMagazines, // 杂志和教辅书
    Diary7_Internet,          // 因特网
}

//  立绘状态
public enum PortraitOption
{
    None,
    Child_Neutral,
    Child_Happy1,
    Child_Happy2,
    Child_Confused,
    Child_Surprised,
    Child_Pout,
    Adult_Neutral,
    Adult_Happy1,
    Adult_Surprised,
    Adult_Angery,
}