using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Refractor_Test 
{
    [Test]
    public void RefractorTest_AngleFortyFive_IndexOnePointFiveTwo()
    {
        GameObject gameObj = new GameObject("whatever");
        Refractor refractor = gameObj.AddComponent<Refractor>();
        RaycastHit2D hitInfo = new RaycastHit2D();
        hitInfo.point = Vector2.zero;
        hitInfo.normal = Vector2.left;

        //NOTE: Ezra: change this angle for future tests
        float angle = 45 * Mathf.Deg2Rad;
        refractor.RefractionIndex = 1.52f;  //glass
        float expectedAngle = 0.483863f;    //in radians (found using snells law)

        float x = Mathf.Cos(angle), y = Mathf.Sin(angle);
        Vector2 inputDir = new Vector2(x, y);
        Vector2[] retArr = refractor.Hit(inputDir, hitInfo, 1);
        bool smallDifference = false;
        if (retArr.Length > 0)
        {
            Vector2 dir = retArr[1] - retArr[0];
            float retAngle = Mathf.Atan(dir.y / dir.x);
            smallDifference = Mathf.Abs(expectedAngle - retAngle) < .0001f;
        }
        else
        {
            Debug.LogError("retArr.Length = 0");
        }


        Assert.IsTrue(smallDifference);
    }

    [Test]
    public void RefractorTest_AngleFifteen_IndexOnePointThree()
    {
        GameObject gameObj = new GameObject("whatever");
        Refractor refractor = gameObj.AddComponent<Refractor>();
        RaycastHit2D hitInfo = new RaycastHit2D();
        hitInfo.point = Vector2.zero;
        hitInfo.normal = Vector2.left;

        //NOTE: Ezra: change this angle for future tests
        float angle = 15 * Mathf.Deg2Rad;
        refractor.RefractionIndex = 1.333f;  //water
        float expectedAngle = 0.195404f;    //in radians (found using snells law)

        float x = Mathf.Cos(angle), y = Mathf.Sin(angle);
        Vector2 inputDir = new Vector2(x, y);
        Vector2[] retArr = refractor.Hit(inputDir, hitInfo, 1);
        bool smallDifference = false;
        if (retArr.Length > 0)
        {
            Vector2 dir = retArr[1] - retArr[0];
            float retAngle = Mathf.Atan(dir.y / dir.x);
            smallDifference = Mathf.Abs(expectedAngle - retAngle) < .0001f;
        }
        else
        {
            Debug.LogError("retArr.Length = 0");
        }


        Assert.IsTrue(smallDifference);
    }


    [Test]
    public void RefractorTest_AngleSixty_IndexTwoPointFourOneNine()
    {
        GameObject gameObj = new GameObject("whatever");
        Refractor refractor = gameObj.AddComponent<Refractor>();
        RaycastHit2D hitInfo = new RaycastHit2D();
        hitInfo.point = Vector2.zero;
        hitInfo.normal = Vector2.left;

        //NOTE: Ezra: change this angle for future tests
        float angle = 60 * Mathf.Deg2Rad;
        refractor.RefractionIndex = 2.419f;  //diamond
        float expectedAngle = 0.3661354f;    //in radians (found using snells law)

        float x = Mathf.Cos(angle), y = Mathf.Sin(angle);
        Vector2 inputDir = new Vector2(x, y);
        Vector2[] retArr = refractor.Hit(inputDir, hitInfo, 1);
        bool smallDifference = false;
        if (retArr.Length > 0)
        {
            Vector2 dir = retArr[1] - retArr[0];
            float retAngle = Mathf.Atan(dir.y / dir.x);
            smallDifference = Mathf.Abs(expectedAngle - retAngle) < .0001f;
        }
        else
        {
            Debug.LogError("retArr.Length = 0");
        }


        Assert.IsTrue(smallDifference);
    }
}
