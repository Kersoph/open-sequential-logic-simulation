using Godot;
using WAT;
using Osls;
using Tests.Core;


namespace Tests.PlantModels
{
    public static class PlantModelRunner
    {
        /// <summary>
        /// Runs the WAT test on the given scene.
        /// the local sfcPath should result in the expectedStars within the expectedTime.
        /// </summary>
        /// <param name="wat">WAT reference test node</param>
        /// <param name="scenePath">the local path to the test scene where the model is running on</param>
        /// <param name="sfcPath">local path to the sfc diagram</param>
        /// <param name="expectedStars">number of stars expected</param>
        /// <param name="expectedTime">maximal calculation rounds to finish</param>
        public static void Test(Test wat, string scenePath, string sfcPath, int expectedStars, int expectedTime)
        {
            PageModule scene = ((PackedScene)GD.Load(scenePath)).Instance() as PageModule;
            LessonEntityMock lessonEntityMock = new LessonEntityMock()
            {
                Stars = -1,
                TemporaryDiagramFilePath = ProjectSettings.GlobalizePath(sfcPath),
            };
            MainNodeMock mainNodeMock = new MainNodeMock();
            scene.InitialiseWith(mainNodeMock, lessonEntityMock);
            wat.AddChild(scene);
            int processingRounds = 0;
            while (lessonEntityMock.Stars == -1 && processingRounds < expectedTime)
            {
                processingRounds++;
                scene._Process(0.05f);
            }
            wat.Assert.IsEqual(lessonEntityMock.Stars, expectedStars, "Rounds: " + processingRounds);
            wat.RemoveChild(scene);
            scene.QueueFree();
        }
    }
}
