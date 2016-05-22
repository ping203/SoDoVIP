using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelRanking : PanelGame {
    public GameObject itemRanking;
    public GameObject parentItem;

    public List<ItemRanking> topPlayers = new List<ItemRanking>();
    //void Awake() {
    //    InstanceItem();
    //}

    //public void InstanceItem() {
    //    if (itemRanking == null) {
    //        Debug.LogError("itemRanking is null!!");
    //    }

    //    if (parentItem == null) {
    //        Debug.LogError("parentItem is null!!");
    //    }

    //    //int childCount = parentItem.transform.childCount;
    //    //if(childCount > 0)
    //    //{
    //    //    foreach (Transform child in parentItem.transform)
    //    //    {
    //    //        Destroy(child);
    //    //    }
    //    //}

    //    if (BaseInfo.gI().topPlayers != null) {
    //        //int nCount = BaseInfo.gI().topPlayers.Count;
    //        for (int i = 0; i < nCount; i++) {
    //            GameObject go = (GameObject)Instantiate(itemRanking);
    //            go.transform.SetParent(parentItem.transform, false);

    //            ItemRanking tempScript = go.GetComponent<ItemRanking>();
    //            if (tempScript != null) {
    //                tempScript.SetData(i + 1, BaseInfo.gI().topPlayers[i].idAvata, BaseInfo.gI().topPlayers[i].playerName,
    //                    BaseInfo.gI().topPlayers[i].money);
    //            }
    //        }
    //    }
    //}

    public void InstanceItem(RankingPlayer item) {
        if (itemRanking == null) {
            Debug.LogError("itemRanking is null!!");
        }

        if (parentItem == null) {
            Debug.LogError("parentItem is null!!");
        }
        GameObject go = (GameObject)Instantiate(itemRanking);
        go.transform.SetParent(parentItem.transform, false);
        go.GetComponent<ItemRanking>().SetData(item.rank, item.idAvata, item.playerName, item.money);

        topPlayers.Add(go.GetComponent<ItemRanking>());
    }

    public void clearList() {
        foreach(ItemRanking it in topPlayers) {
            Destroy(it.gameObject);
        }
        topPlayers.Clear();
        parentItem.GetComponent<UIGrid>().Reposition();
    }
}
