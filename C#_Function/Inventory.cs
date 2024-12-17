# 아이템 인벤토리 저장 및 아이템 누적

// ItemGetter.cs
 IEnumerator GoingTBox(RectTransform itemTransform, RectTransform boxTransform)
 {
 	inventory.AddItem(itemTransform.GetComponent<GettedObject>());
 
 }
 private void OnTriggerEnter2D(Collider2D other)
 {
 	var spawnedItem = other.GetComponent<SpawnedItem>();
        if (spawnedItem != null)
        {
            var itemDataArray = spawnedItem.GetItemData(); // ItemData 배열을 가져옴
            if (itemDataArray != null && itemDataArray.Length > 0)
            {
                newObject.GetComponent<GettedObject>().SetItemData(itemDataArray[0]); 
                // 첫 번째 데이터 전달
            }
        }
 }
ItemGetter 스크립트에서 아이템과 감지 시 OnTriggerEnter2D함수가 호출되어 ItemGetter와 충돌된 물체 즉, 아이템의 SpawnedItem이라는 스크립트를 가져오며
그 ItemData를 통해 item에 대한 정보를 전달하여  인벤토리의 AddItem을 실행합니다.

  //Inventory.cs
public void AddItem(GettedObject item)
{
    ItemData newItemData = item.itemData;

    // 동일한 ItemData가 있는지 확인
    for (var i = 0; i < buttons.Length; i++)
    {
        if (buttons[i].ItemInfo != null && buttons[i].ItemInfo.itemData == newItemData)
        {
            buttons[i].ItemInfo.amount += 1;
            buttons[i].ItemInfo = buttons[i].ItemInfo; // UI 갱신
            return;
        }
    }

    // 빈 슬롯에 새 아이템 추가
    for (var i = 0; i < buttons.Length; i++)
    {
        if (buttons[i].ItemInfo == null)
        {
            buttons[i].ItemInfo = new ItemInfo() { itemData = newItemData, amount = 1 };
            return;
        }
    }
}
AddItem함수에 대한 내용은 전달받은 ItemData에 대해 Inventory 내 빈 공간이 있다면 해당
ItemData를 빈 공간에 넣고 넣으려는 공간에 동일한 데이터가 있다면 누적을 하여 업데이트를 할 수 있습니다.
