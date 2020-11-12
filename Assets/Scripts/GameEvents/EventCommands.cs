using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public partial class EventCommand {
    public class SetPlayerPosition : EventCommandInterface {
        public IEnumerator Do(SharedData shared, SharedVariable variable) {
            var player = shared.Player;
            LevelInfo level = shared.LevelDictionary[LevelManager.GetInstance().CurrentLevelName];
            var entity = shared.ExecuterEntity;

            Vector3 worldPosition = shared.Grid.GetWorldPosition(entity.row, entity.column);
            player.transform.position = worldPosition;
            
            yield break;
        }
    }

    public interface EventCommandInterface {
        IEnumerator Do(SharedData share, SharedVariable variable);
    }
}
