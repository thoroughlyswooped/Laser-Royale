using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TemplateUnitTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void TemplateUnitTestSimplePasses()
    {
        // Use the Assert class to test conditions
        Assert.AreEqual(0, 1 - 1);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TemplateUnitTestWithEnumeratorPasses()
    {
        
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }

    [Test]
    public void RefractorTest_AngleZero_IndexOnePointFiveTwo()
    {
        GameObject gameObj = new GameObject("whatever");
        Refractor refractor = gameObj.AddComponent<Refractor>();
        RaycastHit2D hitInfo = new RaycastHit2D();
        hitInfo.point = Vector2.zero;
        hitInfo.normal = Vector2.left;

        //NOTE: Ezra: change this angle for future tests
        float angle = 45 * Mathf.Deg2Rad;
        refractor.RefractionIndex = 1.52f;  //glass
        float expectedAngle = 0.483863f;    //in radians

        float x = Mathf.Cos(angle), y = Mathf.Sin(angle);
        Vector2 inputDir = new Vector2(x, y); 
        Vector2[] retArr = refractor.Hit(inputDir, hitInfo, 1);
        bool smallDifference = false;
        if (retArr.Length > 0)
        {
            Vector2 dir = retArr[1] - retArr[0];
            float retAngle = Mathf.Atan(dir.y/dir.x);
            smallDifference = Mathf.Abs(expectedAngle - retAngle) < .0001f;
        }
        else
        {
            Debug.LogError("retArr.Length = 0");
        }

        
        Assert.IsTrue(smallDifference);
    }
}
