using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    Bed,          //´²
    NoteBook,     //ÃÜÂë±¾
    Note,         //±ãÀûÌù
    FishTank,     //Óã¸×
    Doll,         //ĞÜÍæÅ¼
    Award,        //½±×´
    Beads,        //´®Öé
    Duck,         //ÏğÆ¤Ñ¼  
    Map,          //µØÍ¼
    Cart          //Ğ¡³Ô³µ
}
public enum RoomState
{
    None,
    // --- S4 ×´Ì¬ ---
    Intro,
    NoteLocked,
    PasswordCollecting,
    AllTasksDone,
    ReadyToExit,
    // --- S6 »ò ÃÎ¾³×´Ì¬ ---
    S6_Bedroom_Intro,
    Dream2_Bedroom,
    ReadyToStreet,
    Dream2_Street,
    Dream2_TaskCompleted,
    ReadyToS7
}
