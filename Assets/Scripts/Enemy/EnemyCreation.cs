using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreation : MonoBehaviour, ISetupable, ILoopable {
    Dictionary<string, ObjectPool<EnemyBase>> _enemyObjectPoolDic;
    List<KeyValuePair<string, EnemyBase>> _activeEnemies;

    public void Initalize() {
        _enemyObjectPoolDic = new Dictionary<string, ObjectPool<EnemyBase>>();
        _activeEnemies = new List<KeyValuePair<string, EnemyBase>>();

        var assetManager = AssetManager.GetInstance();
        var enemies = assetManager.GetComponentsInObjectBundle<EnemyBase>();

        foreach (var enemyPrefab in enemies) {
            var objectPool = new ObjectPool<EnemyBase>(
                5,
                () => {
                    var enemy = Instantiate(enemyPrefab);
                    enemy.Initalize();
                    OnEnemyDisable(enemy);
                    return enemy;
                },
                OnEnemyEnable,
                OnEnemyDisable
            );
            _enemyObjectPoolDic.Add(enemyPrefab.name, objectPool);
        }
    }

    public void Progress() {
        int numOfActiveEnemies = _activeEnemies.Count;
        for (int i = 0; i < numOfActiveEnemies; ++i) {
            _activeEnemies[i].Value.Progress();
        }
    }

    public EnemyBase CreateEnemy(string enemyName) {
        EnemyBase returnEnemy = null;
        if (_enemyObjectPoolDic.TryGetValue(enemyName, out ObjectPool<EnemyBase> objectPool)) {
            returnEnemy = objectPool.GetObject();
            _activeEnemies.Add(new KeyValuePair<string, EnemyBase>(enemyName, returnEnemy));
        }
        else {
            Debug.LogError("Enemy Name Not Exists");
        }
        return returnEnemy;
    }

    void OnEnemyEnable(EnemyBase enemy) {
        enemy.gameObject.SetActive(true);
    }
    void OnEnemyDisable(EnemyBase enemy) {
        enemy.Reset();
    }
}
