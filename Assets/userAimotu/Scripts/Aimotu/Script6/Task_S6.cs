using UnityEngine;
using TMPro;

namespace S6
{
    public class Task_S6 : MonoBehaviour, TaskModule
    {
        public TextMeshProUGUI diaryCountText;
        private const int DIARY_TOTAL = 6;

        public void ShowTaskUI()
        {
            gameObject.SetActive(true);
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (diaryCountText == null) return;
            int found = GlobalData.UnlockedDiaryCount(); // 见下方说明
            diaryCountText.text = $"找回 {found} / {DIARY_TOTAL} 篇日记";
        }

        public bool IsAllCompleted() =>
            GlobalData.UnlockedDiaryCount() >= DIARY_TOTAL;
        public void CollectDiary(int index)
        {
            DiaryID[] slots =
            {
                DiaryID.Diary2_SnackCart,
                DiaryID.Diary3_SummerTV,
                DiaryID.Diary4_SnackGachaToyPhone,
                DiaryID.Diary5_Stationery,
                DiaryID.Diary6_BooksAndMagazines,
                DiaryID.Diary7_Internet,
            };
            if (index < 0 || index >= slots.Length) return;
            GlobalData.UnlockDiary(slots[index]);
            UpdateUI();
        }

     
    }
}

