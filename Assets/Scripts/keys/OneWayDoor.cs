using UnityEngine;

public class OneWayDoor : MonoBehaviour
{
    public int numberOfEnemies;
    private bool needsPrize = false;

    void Update()
    {
        if (numberOfEnemies > 0)
        {
            //print your number of enemies on screen
        }
        else
        {
            needsPrize = true;
            print("You win bla");
        }

        //then, simply check:
        if (needsPrize)
        {
            //Instantiate prize

            //Don't forget this:
            needsPrize = false;
        }
    }
}
