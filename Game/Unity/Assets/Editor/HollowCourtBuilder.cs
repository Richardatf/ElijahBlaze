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

        private enum ShardlingForm { Runner, Bulwark, Harrier }

        [MenuItem("Elijah Blaze/Build Hollow Court Prototype")]
        public static void Build()
        {
            if (!AssetDatabase.IsValidFolder(SceneFolder))
                AssetDatabase.CreateFolder("Assets", "Scenes");

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "HollowCourt";

            Material stone = CreateMaterial("Hollow Basalt", new Color(0.075f, 0.095f, 0.12f));
            Material architecture = CreateMaterial("Living Architecture", new Color(0.16f, 0.27f, 0.34f));
            Material runnerShell = CreateMaterial("Shardling Cinder Shell", new Color(0.36f, 0.12f, 0.055f));
            Material runnerCore = CreateMaterial("Shardling Cinder Heart", new Color(1f, 0.28f, 0.06f), true);
            Material bulwarkShell = CreateMaterial("Shardling Umber Shell", new Color(0.22f, 0.09f, 0.075f));
            Material bulwarkCore = CreateMaterial("Shardling Umber Heart", new Color(0.92f, 0.22f, 0.08f), true);
            Material harrierShell = CreateMaterial("Shardling Slate Shell", new Color(0.16f, 0.22f, 0.25f));
            Material harrierCore = CreateMaterial("Shardling Slate Heart", new Color(0.25f, 0.82f, 0.9f), true);
            Material playerBlue = CreateMaterial("Elijah Blaze Blue Armor", new Color(0.045f, 0.19f, 0.52f));
            Material playerGold = CreateMaterial("Guardian Gold", new Color(0.95f, 0.62f, 0.12f), true);
            Material playerSilver = CreateMaterial("Guardian Pale Blue", new Color(0.38f, 0.72f, 0.94f));
            Material playerFlame = CreateMaterial("Guardian Flame", new Color(1f, 0.22f, 0.035f), true);
            Material shieldMaterial = CreateMaterial("Guardian Defense Field", new Color(0.16f, 0.68f, 1f), true);
            Material gateMaterial = CreateMaterial("Resonance Gate", new Color(0.62f, 0.4f, 0.12f), true);
            Material inlayMaterial = CreateMaterial("Resonance Inlay", new Color(0.22f, 0.48f, 0.54f), true);

            CreateLighting(gateMaterial);
            CreateCourt(stone, architecture, inlayMaterial);

            GameObject player = CreatePlayer(playerBlue, playerGold, playerSilver, playerFlame, shieldMaterial);
            CreateCamera(player.transform);

            var enemyHealth = new List<Health>();
            enemyHealth.Add(CreateShardling(new Vector3(-5f, 0.75f, 2f), player.transform, runnerShell, runnerCore, "Shardling_CinderRunner", ShardlingForm.Runner, 3.4f, 1.0f));
            enemyHealth.Add(CreateShardling(new Vector3(0f, 0.75f, 5f), player.transform, bulwarkShell, bulwarkCore, "Shardling_Bulwark", ShardlingForm.Bulwark, 1.9f, 1.55f));
            enemyHealth.Add(CreateShardling(new Vector3(5f, 0.75f, 2f), player.transform, harrierShell, harrierCore, "Shardling_Harrier", ShardlingForm.Harrier, 2.8f, 0.8f));

            GameObject encounterObject = new GameObject("HollowCourt_Encounter");
            HollowCourtEncounter encounter = encounterObject.AddComponent<HollowCourtEncounter>();
            Transform sealActivation = new GameObject("Seal_Awakening_Point").transform;
            sealActivation.position = new Vector3(0f, 0f, 4.5f);
            GameObject exitMarker = CreateObjectiveMarker();
            encounter.Configure(enemyHealth, player.transform, sealActivation, exitMarker.transform);
            EditorUtility.SetDirty(encounter);

            CreateGate(encounter, gateMaterial);
            CreateCourtHud(encounter);

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
            // The court is a long processional hall with three readable zones:
            // entry, open combat floor, and the sealed resonance threshold.
            CreateBlock("Court_Floor", new Vector3(0f, -0.5f, 2f), new Vector3(26f, 1f, 34f), stone);
            CreateBlock("West_Wall", new Vector3(-13f, 3f, 2f), new Vector3(1f, 7f, 34f), stone);
            CreateBlock("East_Wall", new Vector3(13f, 3f, 2f), new Vector3(1f, 7f, 34f), stone);
            CreateBlock("Back_Wall", new Vector3(0f, 3f, 18.5f), new Vector3(26f, 7f, 1f), stone);

            // Long floor joints give the space scale and lead the eye toward the gate.
            for (int i = -2; i <= 2; i++)
            {
                float x = i * 4f;
                CreateInlay("Floor_Long_Joint_" + (i + 3), new Vector3(x, 0.012f, 2f), new Vector3(0.045f, 0.025f, 32f), inlay);
            }
            float[] crossJoints = { -10f, -5f, 0f, 10f };
            for (int i = 0; i < crossJoints.Length; i++)
                CreateInlay("Floor_Cross_Joint_" + (i + 1), new Vector3(0f, 0.014f, crossJoints[i]), new Vector3(24f, 0.025f, 0.045f), inlay);

            // Repeating ribs make the side walls read as carved architecture instead of a box.
            float[] ribZ = { -10f, -3f, 4f, 11f, 16f };
            for (int i = 0; i < ribZ.Length; i++)
            {
                float height = (i % 2 == 0) ? 6.8f : 5.8f;
                CreateBlock("West_Wall_Rib_" + (i + 1), new Vector3(-12.35f, height * 0.5f, ribZ[i]), new Vector3(0.9f, height, 1.5f), architecture);
                CreateBlock("East_Wall_Rib_" + (i + 1), new Vector3(12.35f, height * 0.5f, ribZ[i]), new Vector3(0.9f, height, 1.5f), architecture);
                CreateBlock("West_Rib_Cap_" + (i + 1), new Vector3(-12.35f, height + 0.18f, ribZ[i]), new Vector3(1.35f, 0.36f, 1.95f), architecture);
                CreateBlock("East_Rib_Cap_" + (i + 1), new Vector3(12.35f, height + 0.18f, ribZ[i]), new Vector3(1.35f, 0.36f, 1.95f), architecture);
            }

            // Entry arch marks the way in and frames the player on a fresh run.
            CreateBlock("Entry_Pier_West", new Vector3(-5.2f, 2.35f, -13.2f), new Vector3(1.1f, 4.7f, 1.5f), architecture);
            CreateBlock("Entry_Pier_East", new Vector3(5.2f, 2.35f, -13.2f), new Vector3(1.1f, 4.7f, 1.5f), architecture);
            CreateBlock("Entry_Lintel", new Vector3(0f, 4.95f, -13.2f), new Vector3(11.5f, 0.9f, 1.5f), architecture);
            CreateInlay("Entry_Crown_Inlay", new Vector3(0f, 5.43f, -13.2f), new Vector3(2.8f, 0.06f, 0.08f), inlay);

            // The encounter has a broad clear center and two looping flank lanes.
            // These offset monoliths provide cover without creating dead ends.
            Vector3[] monolithPositions =
            {
                new Vector3(-7.4f, 1.2f, -2f),
                new Vector3(7.4f, 1.2f, 1f),
                new Vector3(-7.4f, 1.2f, 6f),
                new Vector3(7.4f, 1.2f, 9f)
            };
            for (int i = 0; i < monolithPositions.Length; i++)
            {
                Vector3 position = monolithPositions[i];
                CreateBlock("Court_Monolith_" + (i + 1), position, new Vector3(1.5f, 2.4f, 1.5f), architecture);
                CreateBlock("Court_Monolith_Cap_" + (i + 1), position + new Vector3(0f, 1.28f, 0f), new Vector3(1.9f, 0.22f, 1.9f), inlay);
                CreateInlay("Court_Monolith_Face_" + (i + 1), position + new Vector3(0f, 0.15f, -0.76f), new Vector3(0.16f, 1.45f, 0.045f), inlay);
            }

            // A large inlaid encounter seal occupies the open center without blocking movement.
            Vector3 sealCenter = new Vector3(0f, 0.025f, 4.5f);
            CreateRing("Resonance_Ring_Outer", sealCenter, 6.4f, 0.11f, inlay);
            CreateRing("Resonance_Ring_Middle", sealCenter, 4.7f, 0.075f, inlay);
            CreateRing("Resonance_Ring_Inner", sealCenter, 2.8f, 0.06f, inlay);
            for (int i = 0; i < 8; i++)
            {
                float angle = Mathf.PI * 2f * i / 8f;
                Vector3 spoke = sealCenter + new Vector3(Mathf.Cos(angle) * 3.75f, 0f, Mathf.Sin(angle) * 3.75f);
                GameObject mark = CreateInlay("Seal_Radial_Mark_" + (i + 1), spoke, new Vector3(0.11f, 0.035f, 1.0f), inlay);
                mark.transform.rotation = Quaternion.Euler(0f, 90f - angle * Mathf.Rad2Deg, 0f);
            }

            CreateInlay("Approach_Line_Left", new Vector3(-1.05f, 0.025f, -5f), new Vector3(0.08f, 0.04f, 18f), inlay);
            CreateInlay("Approach_Line_Right", new Vector3(1.05f, 0.025f, -5f), new Vector3(0.08f, 0.04f, 18f), inlay);

            // Three shallow threshold courses lead toward the gate; each remains traversable.
            for (int i = 0; i < 3; i++)
            {
                float stepZ = 10.7f + i * 1.05f;
                float stepHeight = 0.12f + i * 0.08f;
                CreateBlock("Gate_Approach_Course_" + (i + 1), new Vector3(0f, stepHeight * 0.5f, stepZ), new Vector3(8.4f, stepHeight, 0.8f), architecture);
            }

            // Gate frame and crown establish a clear destination silhouette.
            CreateBlock("Threshold_Left", new Vector3(-4.7f, 2.9f, 14.7f), new Vector3(1.1f, 5.8f, 1.7f), architecture);
            CreateBlock("Threshold_Right", new Vector3(4.7f, 2.9f, 14.7f), new Vector3(1.1f, 5.8f, 1.7f), architecture);
            CreateBlock("Threshold_Lintel", new Vector3(0f, 6.0f, 14.7f), new Vector3(10.5f, 1.0f, 1.7f), architecture);
            CreateInlay("Gate_Crown_Inlay", new Vector3(0f, 6.53f, 14.68f), new Vector3(2.7f, 0.08f, 0.08f), inlay);
        }

        private static GameObject CreatePlayer(Material blue, Material gold, Material silver, Material flame, Material shieldMaterial)
        {
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "ElijahBlue_Player";
            player.transform.position = new Vector3(0f, 1f, -7.5f);
            player.GetComponent<Renderer>().enabled = false;
            Object.DestroyImmediate(player.GetComponent<CapsuleCollider>());

            CharacterController controller = player.AddComponent<CharacterController>();
            controller.height = 2f;
            controller.radius = 0.38f;
            player.AddComponent<Health>();
            player.AddComponent<PlayerController>();
            player.AddComponent<PlayerAttack>();

            // A compact blue-and-gold guardian silhouette: armored torso, limbs,
            // crowned helm, rising flame, and a visible trident-like chest mark.
            CreatePlayerPart(player.transform, "Blue_Armored_Torso", PrimitiveType.Capsule, new Vector3(0f, 0.04f, 0f), new Vector3(0.68f, 0.52f, 0.42f), Vector3.zero, blue);
            CreatePlayerPart(player.transform, "Gold_Waist_Guard", PrimitiveType.Cube, new Vector3(0f, -0.34f, 0.015f), new Vector3(0.5f, 0.2f, 0.4f), Vector3.zero, gold);
            CreatePlayerPart(player.transform, "Silver_Chest_Plate", PrimitiveType.Cube, new Vector3(0f, 0.2f, 0.21f), new Vector3(0.39f, 0.42f, 0.065f), Vector3.zero, silver);

            CreatePlayerPart(player.transform, "Left_Gold_Shoulder", PrimitiveType.Sphere, new Vector3(-0.43f, 0.32f, 0f), new Vector3(0.31f, 0.3f, 0.34f), Vector3.zero, gold);
            CreatePlayerPart(player.transform, "Right_Gold_Shoulder", PrimitiveType.Sphere, new Vector3(0.43f, 0.32f, 0f), new Vector3(0.31f, 0.3f, 0.34f), Vector3.zero, gold);
            CreatePlayerPart(player.transform, "Left_Arm", PrimitiveType.Capsule, new Vector3(-0.49f, -0.02f, 0f), new Vector3(0.19f, 0.38f, 0.2f), new Vector3(0f, 0f, -8f), blue);
            CreatePlayerPart(player.transform, "Right_Arm", PrimitiveType.Capsule, new Vector3(0.49f, -0.02f, 0f), new Vector3(0.19f, 0.38f, 0.2f), new Vector3(0f, 0f, 8f), blue);
            CreatePlayerPart(player.transform, "Left_Gauntlet", PrimitiveType.Cube, new Vector3(-0.52f, -0.31f, 0.04f), new Vector3(0.22f, 0.16f, 0.24f), Vector3.zero, gold);
            CreatePlayerPart(player.transform, "Right_Gauntlet", PrimitiveType.Cube, new Vector3(0.52f, -0.31f, 0.04f), new Vector3(0.22f, 0.16f, 0.24f), Vector3.zero, gold);

            CreatePlayerPart(player.transform, "Left_Leg", PrimitiveType.Capsule, new Vector3(-0.2f, -0.58f, 0f), new Vector3(0.22f, 0.4f, 0.24f), Vector3.zero, blue);
            CreatePlayerPart(player.transform, "Right_Leg", PrimitiveType.Capsule, new Vector3(0.2f, -0.58f, 0f), new Vector3(0.22f, 0.4f, 0.24f), Vector3.zero, blue);
            CreatePlayerPart(player.transform, "Left_Greave", PrimitiveType.Cube, new Vector3(-0.2f, -0.61f, 0.15f), new Vector3(0.19f, 0.34f, 0.1f), Vector3.zero, gold);
            CreatePlayerPart(player.transform, "Right_Greave", PrimitiveType.Cube, new Vector3(0.2f, -0.61f, 0.15f), new Vector3(0.19f, 0.34f, 0.1f), Vector3.zero, gold);
            CreatePlayerPart(player.transform, "Left_Boot", PrimitiveType.Cube, new Vector3(-0.2f, -0.89f, 0.06f), new Vector3(0.3f, 0.16f, 0.42f), Vector3.zero, silver);
            CreatePlayerPart(player.transform, "Right_Boot", PrimitiveType.Cube, new Vector3(0.2f, -0.89f, 0.06f), new Vector3(0.3f, 0.16f, 0.42f), Vector3.zero, silver);

            CreatePlayerPart(player.transform, "Guardian_Helm", PrimitiveType.Sphere, new Vector3(0f, 0.76f, 0f), new Vector3(0.39f, 0.42f, 0.35f), Vector3.zero, blue);
            CreatePlayerPart(player.transform, "Gold_Helm_Brow", PrimitiveType.Cube, new Vector3(0f, 0.68f, 0.29f), new Vector3(0.44f, 0.08f, 0.06f), Vector3.zero, gold);
            CreatePlayerPart(player.transform, "Helm_Crest", PrimitiveType.Cube, new Vector3(0f, 1.04f, -0.02f), new Vector3(0.13f, 0.35f, 0.16f), new Vector3(12f, 0f, 0f), gold);
            CreatePlayerPart(player.transform, "Rising_Flame", PrimitiveType.Sphere, new Vector3(0f, 1.29f, -0.02f), new Vector3(0.2f, 0.34f, 0.16f), new Vector3(0f, 0f, -12f), flame);

            // The three bars form a small trident mark centered on the breastplate.
            CreatePlayerPart(player.transform, "Chest_Sigil_Stem", PrimitiveType.Cube, new Vector3(0f, 0.18f, 0.255f), new Vector3(0.045f, 0.29f, 0.035f), Vector3.zero, gold);
            CreatePlayerPart(player.transform, "Chest_Sigil_Left", PrimitiveType.Cube, new Vector3(-0.075f, 0.31f, 0.255f), new Vector3(0.04f, 0.17f, 0.035f), new Vector3(0f, 0f, -35f), gold);
            CreatePlayerPart(player.transform, "Chest_Sigil_Right", PrimitiveType.Cube, new Vector3(0.075f, 0.31f, 0.255f), new Vector3(0.04f, 0.17f, 0.035f), new Vector3(0f, 0f, 35f), gold);

            GameObject guardVisual = CreatePlayerPart(player.transform, "Guardian_Guard_Field", PrimitiveType.Sphere, new Vector3(0f, 0.05f, 0.64f), new Vector3(1.15f, 1.72f, 0.2f), Vector3.zero, shieldMaterial);
            guardVisual.SetActive(false);
            PlayerDefense defense = player.AddComponent<PlayerDefense>();
            defense.Configure(guardVisual);
            player.AddComponent<PlayerDeathController>();
            EditorUtility.SetDirty(defense);
            return player;
        }

        private static GameObject CreatePlayerPart(
            Transform parent,
            string name,
            PrimitiveType primitive,
            Vector3 localPosition,
            Vector3 localScale,
            Vector3 localRotation,
            Material material)
        {
            GameObject part = GameObject.CreatePrimitive(primitive);
            part.name = name;
            Object.DestroyImmediate(part.GetComponent<Collider>());
            part.transform.SetParent(parent, false);
            part.transform.localPosition = localPosition;
            part.transform.localRotation = Quaternion.Euler(localRotation);
            part.transform.localScale = localScale;
            part.GetComponent<Renderer>().sharedMaterial = material;
            return part;
        }

        private static void CreateCamera(Transform target)
        {
            GameObject cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.fieldOfView = 62f;
            cameraObject.AddComponent<AudioListener>();
            cameraObject.transform.position = target.position + new Vector3(0f, 3.8f, -4.5f);
            CameraFollow follow = cameraObject.AddComponent<CameraFollow>();
            follow.SetTarget(target);
            EditorUtility.SetDirty(follow);
        }

        private static Health CreateShardling(
            Vector3 position,
            Transform target,
            Material shellMaterial,
            Material coreMaterial,
            string name,
            ShardlingForm form,
            float moveSpeed,
            float attackCooldown)
        {
            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = name;
            enemy.transform.position = position;
            enemy.GetComponent<Renderer>().enabled = false;

            CapsuleCollider bodyCollider = enemy.GetComponent<CapsuleCollider>();
            bodyCollider.radius = 0.43f;
            bodyCollider.height = 1.55f;

            AddShardlingForm(enemy.transform, form, shellMaterial, coreMaterial);

            Health health = enemy.AddComponent<Health>();
            EnemyChase chase = enemy.AddComponent<EnemyChase>();
            EnemyAttack attack = enemy.AddComponent<EnemyAttack>();
            enemy.AddComponent<DeathFeedback>();
            enemy.AddComponent<EnemyHitFeedback>();
            chase.SetTarget(target);
            chase.Configure(moveSpeed, 1.7f);
            attack.SetTarget(target);
            attack.Configure(1.95f, attackCooldown, 1);
            EditorUtility.SetDirty(chase);
            EditorUtility.SetDirty(attack);
            return health;
        }

        private static void AddShardlingForm(Transform root, ShardlingForm form, Material shell, Material core)
        {
            // Each creature is assembled from angular stone fragments around a visible living heart.
            CreateShardPart(root, "Fractured_Core", PrimitiveType.Cube, new Vector3(0f, 0.02f, 0f), new Vector3(0.52f, 0.78f, 0.48f), new Vector3(8f, 45f, 12f), shell);
            CreateShardPart(root, "Living_Heart", PrimitiveType.Sphere, new Vector3(0f, 0.08f, 0.31f), new Vector3(0.2f, 0.24f, 0.12f), Vector3.zero, core);

            if (form == ShardlingForm.Runner)
            {
                CreateShardPart(root, "Runner_Left_Fin", PrimitiveType.Cube, new Vector3(-0.39f, 0.16f, -0.04f), new Vector3(0.3f, 0.78f, 0.3f), new Vector3(0f, 0f, -28f), shell);
                CreateShardPart(root, "Runner_Right_Fin", PrimitiveType.Cube, new Vector3(0.39f, 0.16f, -0.04f), new Vector3(0.3f, 0.78f, 0.3f), new Vector3(0f, 0f, 28f), shell);
                CreateShardPart(root, "Runner_Crown", PrimitiveType.Cube, new Vector3(0f, 0.72f, -0.02f), new Vector3(0.2f, 0.42f, 0.22f), new Vector3(0f, 0f, 12f), shell);
                CreateShardPart(root, "Runner_Tail_Shards", PrimitiveType.Cube, new Vector3(0f, -0.35f, -0.42f), new Vector3(0.22f, 0.52f, 0.24f), new Vector3(35f, 0f, 0f), shell);
            }
            else if (form == ShardlingForm.Bulwark)
            {
                CreateShardPart(root, "Bulwark_Left_Shoulder", PrimitiveType.Cube, new Vector3(-0.48f, 0.28f, 0f), new Vector3(0.52f, 0.56f, 0.52f), new Vector3(0f, 0f, -12f), shell);
                CreateShardPart(root, "Bulwark_Right_Shoulder", PrimitiveType.Cube, new Vector3(0.48f, 0.28f, 0f), new Vector3(0.52f, 0.56f, 0.52f), new Vector3(0f, 0f, 12f), shell);
                CreateShardPart(root, "Bulwark_Left_Arm", PrimitiveType.Cube, new Vector3(-0.55f, -0.25f, 0.04f), new Vector3(0.3f, 0.68f, 0.34f), new Vector3(0f, 0f, -18f), shell);
                CreateShardPart(root, "Bulwark_Right_Arm", PrimitiveType.Cube, new Vector3(0.55f, -0.25f, 0.04f), new Vector3(0.3f, 0.68f, 0.34f), new Vector3(0f, 0f, 18f), shell);
                CreateShardPart(root, "Bulwark_Crown", PrimitiveType.Cube, new Vector3(0f, 0.72f, -0.02f), new Vector3(0.28f, 0.3f, 0.28f), new Vector3(0f, 45f, 0f), shell);
            }
            else
            {
                CreateShardPart(root, "Harrier_Left_Blade", PrimitiveType.Cube, new Vector3(-0.43f, 0.05f, 0f), new Vector3(0.2f, 1.0f, 0.28f), new Vector3(0f, 0f, -20f), shell);
                CreateShardPart(root, "Harrier_Right_Blade", PrimitiveType.Cube, new Vector3(0.43f, 0.05f, 0f), new Vector3(0.2f, 1.0f, 0.28f), new Vector3(0f, 0f, 20f), shell);
                CreateShardPart(root, "Harrier_Crest", PrimitiveType.Cube, new Vector3(0f, 0.8f, -0.04f), new Vector3(0.18f, 0.56f, 0.2f), new Vector3(18f, 0f, 0f), shell);
                CreateShardPart(root, "Harrier_Lower_Shard", PrimitiveType.Cube, new Vector3(0f, -0.47f, 0.02f), new Vector3(0.38f, 0.28f, 0.34f), new Vector3(0f, 45f, 0f), shell);
            }
        }

        private static GameObject CreateShardPart(
            Transform parent,
            string name,
            PrimitiveType primitive,
            Vector3 localPosition,
            Vector3 localScale,
            Vector3 localRotation,
            Material material)
        {
            GameObject part = GameObject.CreatePrimitive(primitive);
            part.name = name;
            Object.DestroyImmediate(part.GetComponent<Collider>());
            part.transform.SetParent(parent, false);
            part.transform.localPosition = localPosition;
            part.transform.localRotation = Quaternion.Euler(localRotation);
            part.transform.localScale = localScale;
            part.GetComponent<Renderer>().sharedMaterial = material;
            return part;
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

        private static GameObject CreateObjectiveMarker()
        {
            GameObject marker = new GameObject("Objective_ReachTheGate");
            marker.transform.position = new Vector3(0f, 0f, 13f);
            return marker;
        }

        private static void CreateCourtHud(HollowCourtEncounter encounter)
        {
            GameObject hudObject = new GameObject("HollowCourt_HUD", typeof(RectTransform));
            hudObject.AddComponent<Canvas>();

            HollowCourtHud hud = hudObject.AddComponent<HollowCourtHud>();
            hud.Configure(encounter);
            EditorUtility.SetDirty(hud);
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

        private static GameObject CreateInlay(string name, Vector3 position, Vector3 scale, Material material)
        {
            GameObject inlay = GameObject.CreatePrimitive(PrimitiveType.Cube);
            inlay.name = name;
            inlay.transform.position = position;
            inlay.transform.localScale = scale;
            Object.DestroyImmediate(inlay.GetComponent<Collider>());
            inlay.GetComponent<Renderer>().sharedMaterial = material;
            return inlay;
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
