using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public partial class EventCommand {
    public class SetPlayerPosition : EventCommandInterface {
        public IEnumerator Execute(SharedData data, SharedVariable variable) {
            var player = data.Player;
            LevelInfo level = data.LevelDictionary[LevelManager.GetInstance().CurrentLevelName];
            var entity = data.ExecuterEntity;

            Vector3 worldPosition = data.Grid.GetWorldPosition(entity.row, entity.column);
            player.transform.position = worldPosition;
            
            yield break;
        }
    }

    public interface EventCommandInterface {
        IEnumerator Execute(SharedData data, SharedVariable variable);
    }
}
