using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicUnit : Unit
{
    public string spriteCategory;
    public string spriteName;

    public bool useGeneralSpawnAnim;
    public bool useGeneralDeathAnim;

    public const string SPAWN = "spawn";
    public const string DEATH = "death";
    public const string TRANSFORM = "transform";

    public const string GENERAL_CATEGORY = "general";
    public const string GENERAL_NAME = "sprite";

    //

    //protected override void Start()
    //{
    //    StartCoroutine(DelayedSpawn());
    //}

    //protected virtual IEnumerator DelayedSpawn()
    //{
    //    float wait = 0f;
    //    if(useGeneralSpawnAnim)
    //    {
    //        SpriteManager.SpriteAttribute spawnSprite = SpriteManager.GetSpriteAttribute(GENERAL_CATEGORY, GENERAL_NAME, SPAWN);
    //        wait = SpriteManager.AssignSpriteAttribute(gameObject, spawnSprite);
    //    }
    //    else
    //    {
    //        SpriteManager.SpriteAttribute spawnSprite = SpriteManager.GetSpriteAttribute(spriteCategory, spriteName, SPAWN);
    //        wait = SpriteManager.AssignSpriteAttribute(gameObject, spawnSprite);
    //    }

    //    Activated = false;

    //    yield return new WaitForSeconds(wait);

    //    SpriteManager.SpriteAttribute commonSprite = SpriteManager.GetSpriteAttribute(spriteCategory, spriteName);
    //    SpriteManager.AssignSpriteAttribute(gameObject, commonSprite);

    //    Activated = true;

    //    OnSpawn();
    //}

    //public override void OnDeath()
    //{
    //    StartCoroutine(DelayedDeath());
    //}

    //protected virtual IEnumerator DelayedDeath()
    //{
    //    float wait = 0f;
    //    if (useGeneralDeathAnim)
    //    {
    //        SpriteManager.SpriteAttribute spawnSprite = SpriteManager.GetSpriteAttribute(GENERAL_CATEGORY, GENERAL_NAME, DEATH);
    //        wait = SpriteManager.AssignSpriteAttribute(gameObject, spawnSprite);
    //    }
    //    else
    //    {
    //        SpriteManager.SpriteAttribute spawnSprite = SpriteManager.GetSpriteAttribute(spriteCategory, spriteName, DEATH);
    //        wait = SpriteManager.AssignSpriteAttribute(gameObject, spawnSprite);
    //    }

    //    Activated = false;

    //    yield return new WaitForSeconds(wait);

    //    Destroy(gameObject);
    //}

    //public virtual void OnTransform()
    //{
    //    StartCoroutine(DelayedTransform());
    //}

    //protected virtual IEnumerator DelayedTransform()
    //{
    //    SpriteManager.SpriteAttribute spawnSprite = SpriteManager.GetSpriteAttribute(spriteCategory, spriteName, TRANSFORM);
    //    float wait = SpriteManager.AssignSpriteAttribute(gameObject, spawnSprite);

    //    Activated = false;

    //    yield return new WaitForSeconds(wait);

    //    SpriteManager.SpriteAttribute commonSprite = SpriteManager.GetSpriteAttribute(spriteCategory, spriteName);
    //    SpriteManager.AssignSpriteAttribute(gameObject, commonSprite);

    //    Activated = true;

    //    OnSpawn();
    //}

    //
}
