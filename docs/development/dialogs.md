# Adding dialogs

## Step 1 (Add dialog info)
1. Review this file to see the data format of the dialog [DialogInfo.cs](../../Blasphemous.LostDreams/Dialog/DialogInfo.cs)
2. Navigate to the resources/data/Lost Dreams/dialogs.json file
3. Add a dialog object with the following properties to the json file:

| Name | Description | Required |
| ---- | ----------- | :------: |
| id | The name of the dialog (Must be unique) | yes |
| type | "Lines", "GiveObject", "GivePurge", "BuyObject", "PurgeGeneric" | yes |
| item | Id of the item to give or take | no |

4. Copy the json into https://jsonlint.com to validate it, then copy it back into the json file to format it

## Step 2 (Add localization)
1. Navigate to the resources/localization/Lost Dreams.txt file
2. Add a required key ```{DIALOG_ID}.text``` which contains the list of text lines separated by '@'
3. Add an optional key ```{DIALOG_ID}.resp``` which contains the list of response lines separated by '@'

---

Example "dialogs.json"
```json
[
    {
        "id": "NPC01_meet",
        "type": "Lines"
    },
    {
        "id": "NPC01_item",
        "type": "BuyObject",
        "item": "PR501"
    },
    {
        "id": "NPC01_common",
        "type": "Lines"
    }
]
```

Example "Lost Dreams.txt"
```
NPC01_meet.text: This is the first line of when you first talk to them @ This will only ever play once

NPC01_item.text: Take this item
NPC01_meet.resp: Answer yes @ Answer no

NPC01_common: This dialog can be used to play on repeat
```
