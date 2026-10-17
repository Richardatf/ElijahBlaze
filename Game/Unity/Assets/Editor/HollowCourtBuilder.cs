using System.Collections.Generic;
using ElijahBlaze.CameraSystem;
using ElijahBlaze.Combat;
using ElijahBlaze.Core;
using ElijahBlaze.Enemies;
using ElijahBlaze.Player;
using ElijahBlaze.World;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ElijahBlaze.EditorTools
{
    public static class HollowCourtBuilder
    {
        private const string SceneFolder = "Assets/Scenes";
        private const string ScenePath = SceneFolder + "/HollowCourt.unity";

        [MenuItem("Elijah Blaze/Build Hollow Court Prototype")]
        public static void Build()
        {
            if (!AssetDatabase.IsValidFolder(SceneFolder))
                AssetDatabase.CreateFolder("Assets", "Scenes");

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "HollowCourt";

            Material stone = CreateMaterial("Hollow Stone", new Color(0.09f, 0.12f, 0.15f));
            Material architecture = CreateMaterial("Living Architecture", new Color(0.12f, 0.23f, 0.33f));
            Material shard = CreateMaterial("Shardling", new Color(0.25f, 0.08f, 0.06f));
            Material playerMaterial = CreateMaterial("Elijah Blue", new Color(0.08f, 0.25f, 0.55f));
            Material gateMaterial = CreateMaterial("Resonance Gate", new Color(0.58f, 0.38f, 0.08f));

            CreateLighting();
            CreateCourt(stone, architecture);

            GameObject player = CreatePlayer(playerMaterial);
            CreateCamera(player.transform);

            var enemyHealth = new List<Health>();
            enemyHealth.Add(CreateShardling(new Vector3(-5f, 1f, 2f), player.transform, shard, "Shardling_A"));
            enemyHealth.Add(CreateShardling(new Vector3(0f, 1f, 5f), player.transform, shard, "Shardling_B"));
            enemyHealth.Add(CreateShardling(new Vector3(5f, 1f, 2f), player.transform, shard, "Shardling_C"));

            GameObject encounterObject = new GameObject("HollowCourt_Encounter");
            HollowCourtEncounter encounter = encounterObject.AddComponent<HollowCourtEncounter>();
            encounter.Configure(enemyHealth);
            EditorUtility.SetDirty(encounter);

            CreateGate(encounter, gateMaterial);
            CreateObjectiveMarker();

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Selection.activeGameObject = player;
            Debug.Log("Hollow Court prototype built at " + ScenePath);
        }

        private static void CreateLighting()
        {
            GameObject lightObject = new GameObject("Directional Light");
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.15f;
            lightObject.transform.rotation = Quaternion.Euler(42f, -28f, 0f);

            RenderSettings.ambientLight = new Color(0.16f, 0.19f, 0.23f);
        }

        private static void CreateCourt(Material stone, Material architecture)
        {
            CreateBlock("Court_Floor", new Vector3(0f, -0.5f, 2f), new Vector3(26f, 1f, 34f), stone);
            CreateBlock("West_Wall", new Vector3(-13f, 3f, 2f), new Vector3(1f, 7f, 34f), stone);
            CreateBlock("East_Wall", new Vector3(13f, 3f, 2f), new Vector3(1f, 7f, 34f), stone);
            CreateBlock("Back_Wall", new Vector3(0f, 3f, 18.5f), new Vector3(26f, 7f, 1f), stone);

            for (int i = -2; i <= 2; i++)
            {
                float x = i * 4.5f;
                CreateBlock("Architecture_Rib_" + (i + 3), new Vector3(x, 2f, 8f), new Vector3(0.55f, 4f, 0.55f), architecture);
            }

            CreateBlock("Threshold_Left", new Vector3(-5.5f, 2.5f, 14f), new Vector3(2f, 5f, 2f), architecture);
            CreateBlock("Threshold_Right", new Vector3(5.5f, 2.5f, 14f), new Vector3(2f, 5f, 2f), architecture);
        }

        private static GameObject CreatePlayer(Material material)
        {
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "ElijahBlue_Player";
            player.transform.position = new Vector3(0f, 1f, -10f);
            Object.DestroyImmediate(player.GetComponent<CapsuleCollider>());
            player.GetComponent<Renderer>().sharedMaterial = material;
            player.AddComponent<CharacterController>();
            player.AddComponent<Health>();
            player.AddComponent<PlayerController>();
            player.AddComponent<PlayerAttack>();
            player.AddComponent<PlayerDeathController>();
            return player;
        }

        private static void CreateCamera(Transform target)
        {
            GameObject cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.fieldOfView = 62f;
            cameraObject.AddComponent<AudioListener>();
            cameraObject.transform.position = target.position + new Vector3(0f, 4.5f, -7f);
            CameraFollow follow = cameraObject.AddComponent<CameraFollow>();
            follow.SetTarget(target);
            EditorUtility.SetDirty(follow);
        }

        private static Health CreateShardling(Vector3 position, Transform target, Material material, string name)
        {
            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = name;
            enemy.transform.position = position;
            enemy.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            enemy.GetComponent<Renderer>().sharedMaterial = material;

            Health health = enemy.AddComponent<Health>();
            EnemyChase chase = enemy.AddComponent<EnemyChase>();
            EnemyAttack attack = enemy.AddComponent<EnemyAttack>();
            enemy.AddComponent<DeathFeedback>();
            chase.SetTarget(target);
            attack.SetTarget(target);
            EditorUtility.SetDirty(chase);
            EditorUtility.SetDirty(attack);
            return health;
        }

        private static void CreateGate(HollowCourtEncounter encounter, Material material)
        {
            GameObject gateRoot = new GameObject("Resonance_Gate");
            gateRoot.transform.position = new Vector3(0f, 0f, 15f);

            GameObject gateBody = CreateBlock("Gate_Body", new Vector3(0f, 2.5f, 15f), new Vector3(7f, 5f, 0.8f), material);
            gateBody.transform.SetParent(gateRoot.transform, true);

            ResonanceGate gate = gateRoot.AddComponent<ResonanceGate>();
            gate.Configure(encounter, gateBody.transform, new Vector3(0f, 6f, 0f));
            EditorUtility.SetDirty(gate);
        }

        private static void CreateObjectiveMarker()
        {
            GameObject marker = new GameObject("Objective_ReachTheGate");
            marker.transform.position = new Vector3(0f, 0f, 13f);
        }

        private static GameObject CreateBlock(string name, Vector3 position, Vector3 scale, Material material)
        {
            GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
            block.name = name;
            block.transform.position = position;
            block.transform.localScale = scale;
            block.GetComponent<Renderer>().sharedMaterial = material;
            return block;
        }

        private static Material CreateMaterial(string name, Color color)
        {
            Shader shader = Shader.Find("Standard");
            if (shader == null)
                shader = Shader.Find("Universal Render Pipeline/Lit");

            Material material = new Material(shader) { name = name, color = color };
            return material;
        }
    }
}
