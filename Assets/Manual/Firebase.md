# Firebase 使用方法

---

## 插入一筆新的資料到資料庫
要包含的欄位
- Score
- User
- Time

### API
```c#
CollectionReference colRef = GameManager.Instance.Db.Collection ("ScoreBoard");
Dictionary<string, object> user = new Dictionary<string, object> { { "Time", System.DateTime.Now.ToShortDateString ( ) + " " + System.DateTime.Now.ToShortTimeString ( ) },
    { "Score", elapsedTime },
    { "User", SLController.GetUserName ( ) },
    { "Level", "Level " + (int) (GameManager.Instance.CurrentLevel) }
};
colRef.Document ( ).SetAsync (user).ContinueWith (t => {
    Debug.Log ("Finish add data to firebase");
});
```
---
## 從資料庫讀取資料

### API

```c#
void GetScore ( ) {
    SLController.WriteLog ("Try to get score");
    CollectionReference scoreboardRef = GameManager.Instance.Db.Collection ("ScoreBoard");
    Query query = scoreboardRef.OrderBy ("Score").Limit (10);
    query.GetSnapshotAsync ( ).ContinueWith (task => {
        List<Dictionary<string, object>> data = new List<Dictionary<string, object>> ( );
        foreach (DocumentSnapshot o in task.Result.Documents) {
            data.Add (o.ToDictionary ( ));
        }
        for (int i = 0; i < task.Result.Count; i++) {
            this.datas.Add (new ScoreData (data[i]["User"].ToString ( ), data[i]["Score"].ToString ( ), data[i]["Time"].ToString ( ), data[i]["Level"].ToString ( )));
        }
        bSocreGet = true;
        SLController.WriteLog ("Alreay get the score");
    });
}
```

### 參考資料
- [Get data](https://firebase.google.com/docs/firestore/query-data/get-data)
- [Limit & Sort Data](https://firebase.google.com/docs/firestore/query-data/order-limit-data)

---

## 注意事項
- ContinueWith中無法使用任何有關Unity的API
    - 目前無法確定為什麼 或許是執行階段的不同
    - 但可以使用`Debug.Log`
- 在Build以後有一定的機率會造成應用程式閃退
    - 理由不確定
    - 暫時的解決辦法 將目標平台改成x86 Build以後 在改回x86 x64 重新編譯



