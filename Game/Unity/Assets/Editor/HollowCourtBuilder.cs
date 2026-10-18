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

            Material stone = CreateMaterial("Hollow Basalt", new Color(0.075f, 0.095f, 0.12f));
            Material architecture = CreateMaterial("Living Architecture", new Color(0.16f, 0.27f, 0.34f));
            Material shard = CreateMaterial("Shardling", new Color(0.34f, 0.12f, 0.075f));
            Material playerMaterial = CreateMaterial("Elijah Blue", new Color(0.08f, 0.3f, 0.68f));
            Material gateMaterial = CreateMaterial("Resonance Gate", new Color(0.62f, 0.4f, 0.12f), true);
            Material inlayMaterial = CreateMaterial("Resonance Inlay", new Color(0.22f, 0.48f, 0.54f), true);

            CreateLighting(gateMaterial);
            CreateCourt(stone, architecture, inlayMaterial);

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

        private static void CreateLighting(Material gateMaterial)
        {
            GameObject lightObject = new GameObject("Directional Light");
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = new Color(0.78f, 0.84f, 0.92f);
            light.intensity = 1.25f;
            lightObject.transform.rotation = Quaternion.Euler(42f, -28f, 0f);

            RenderSettings.ambientLight = new Color(0.22f, 0.25f, 0.3f);
            CreatePointLight("Gate_Warmth", new Vector3(0f, 4.6f, 13f), new Color(1f, 0.67f, 0.28f), 2.2f, 12f);
            CreatePointLight("Court_West_Light", new Vector3(-10f, 4f, 1f), new Color(0.4f, 0.66f, 0.78f), 1.1f, 13f);
            CreatePointLight("Court_East_Light", new Vector3(10f, 4f, 8f), new Color(0.4f, 0.66f, 0.78f), 1.1f, 13f);
        }

        private static void CreatePointLight(string name, Vector3 position, Color color, float intensity, float range)
        {
            GameObject lightObject = new GameObject(name);
            lightObject.transform.position = position;
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = color;
            light.intensity = intensity;
            light.range = range;
        }

        private static void CreateCourt(Material stone, Material architecture, Material inlay)
        {
            CreateBlock("Court_Floor", new Vector3(0f, -0.5f, 2f), new Vector3(26f, 1f, 34f), stone);
            CreateBlock("West_Wall", new Vector3(-13f, 3f, 2f), new Vector3(1f, 7f, 34f), stone);
            CreateBlock("East_Wall", new Vector3(13f, 3f, 2f), new Vector3(1f, 7f, 34f), stone);
            CreateBlock("Back_Wall", new Vector3(0f, 3f, 18.5f), new Vector3(26f, 7f, 1f), stone);

            // Wall buttresses define the room's scale while keeping the combat lane clear.
            float[] buttressZ = { -10f, -2f, 6f, 14f };
            for (int i = 0; i < buttressZ.Length; i++)
            {
                CreateBlock("West_Buttress_" + (i + 1), new Vector3(-11.9f, 3.2f, buttressZ[i]), new Vector3(1.2f, 6.4f, 1.8f), architecture);
                CreateBlock("East_Buttress_" + (i + 1), new Vector3(11.9f, 3.2f, buttressZ[i]), new Vector3(1.2f, 6.4f, 1.8f), architecture);
            }

            // Low monoliths create readable flank routes and brief cover without sealing the center.
            Vector3[] monolithPositions =
            {
                new Vector3(-7f, 0.9f, -3f),
                new Vector3(7f, 0.9f, 0f),
                new Vector3(-7f, 0.9f, 5f),
                new Vector3(7f, 0.9f, 8f),
                new Vector3(-7f, 0.9f, 11f)
            };
            for (int i = 0; i < monolithPositions.Length; i++)
                CreateBlock("Court_Monolith_" + (i + 1), monolithPositions[i], new Vector3(1.8f, 1.8f, 1.4f), architecture);

            // Inlaid rings mark the encounter space; the straight center approach remains open.
            CreateRing("Resonance_Ring_Outer", new Vector3(0f, 0.025f, 5f), 6.1f, 0.09f, inlay);
            CreateRing("Resonance_Ring_Inner", new Vector3(0f, 0.03f, 5f), 3.4f, 0.055f, inlay);
            CreateInlay("Approach_Line_Left", new Vector3(-1.05f, 0.025f, -5f), new Vector3(0.08f, 0.04f, 18f), inlay);
            CreateInlay("Approach_Line_Right", new Vector3(1.05f, 0.025f, -5f), new Vector3(0.08f, 0.04f, 18f), inlay);

            // The threshold frames the gate as the destination and leaves a broad route beneath it.
            CreateBlock("Threshold_Left", new Vector3(-4.15f, 2.7f, 14.7f), new Vector3(0.9f, 5.4f, 1.5f), architecture);
            CreateBlock("Threshold_Right", new Vector3(4.15f, 2.7f, 14.7f), new Vector3(0.9f, 5.4f, 1.5f), architecture);
            CreateBlock("Threshold_Lintel", new Vector3(0f, 5.55f, 14.7f), new Vector3(9.2f, 0.9f, 1.5f), architecture);
            CreateInlay("Gate_Crown_Inlay", new Vector3(0f, 6.03f, 14.68f), new Vector3(2.1f, 0.07f, 0.08f), inlay);
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
            CreateInlay("Gate_Sigil_Vertical", new Vector3(0f, 5.1f, 14.48f), new Vector3(0.12f, 1.6f, 0.06f), material);

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

        private static void CreateInlay(string name, Vector3 position, Vector3 scale, Material material)
        {
            GameObject inlay = GameObject.CreatePrimitive(PrimitiveType.Cube);
            inlay.name = name;
            inlay.transform.position = position;
            inlay.transform.localScale = scale;
            Object.DestroyImmediate(inlay.GetComponent<Collider>());
            inlay.GetComponent<Renderer>().sharedMaterial = material;
        }

        private static void CreateRing(string name, Vector3 center, float radius, float width, Material material)
        {
            GameObject ring = new GameObject(name);
            ring.transform.position = center;
            LineRenderer line = ring.AddComponent<LineRenderer>();
            line.useWorldSpace = false;
            line.loop = true;
            line.positionCount = 64;
            line.startWidth = width;
            line.endWidth = width;
            line.sharedMaterial = material;
            line.startColor = material.color;
            line.endColor = material.color;
            for (int i = 0; i < line.positionCount; i++)
            {
                float angle = Mathf.PI * 2f * i / line.positionCount;
                line.SetPosition(i, new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius));
            }
        }

        private static Material CreateMaterial(string name, Color color, bool emissive = false)
        {
            Shader shader = Shader.Find("Standard");
            if (shader == null)
                shader = Shader.Find("Universal Render Pipeline/Lit");

            Material material = new Material(shader) { name = name, color = color };
            material.SetFloat("_Metallic", 0.18f);
            material.SetFloat("_Glossiness", 0.38f);
            if (emissive)
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", color * 0.3f);
            }
            return material;
        }
    }
}
