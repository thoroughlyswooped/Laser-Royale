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

    //TODO: Ezra: finish righting the test for the refractor
    [UnityTest]
    public void RefractorTest_AngleZero_IndexPointFive()
    {
        Refractor refractor = new Refractor();
        RaycastHit2D hitInfo = new RaycastHit2D();
        float x = 0, y = 0;
        Vector2 inputDir = new Vector2(x, y);
        Vector2[] retArr = refractor.Hit(inputDir, hitInfo, 2);
    }
}
