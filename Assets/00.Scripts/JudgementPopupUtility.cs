using System.Linq;
using UnityEngine;

public static class JudgementPopupUtility
{
    private static JudgementSpriteDB _judgementSpriteDB = null;

    public static void Popup(Vector2 position, EJudgement judgement)
    {
        if (PoolManager.Instance == null)
        {
            Debug.LogError("PoolManager Instance Null");
            return;
        }
        if (_judgementSpriteDB == null)
        {
            _judgementSpriteDB = Resources.Load("SO\\JudgementSpriteDB") as JudgementSpriteDB;
            if (_judgementSpriteDB == null)
            {
                Debug.LogError("JudgementSpriteDB Null");
                return;
            }
        }
        JudgementPopupPoolable popup = PoolManager.Instance.Pop(EPoolType.JudgementPopup) as JudgementPopupPoolable;
        popup.SetSprite(_judgementSpriteDB.judgementSpriteDatas.First(x => x.judgement == judgement).sprite);
        popup.transform.position = position;
        popup.PopupAnimation();
    }
}
