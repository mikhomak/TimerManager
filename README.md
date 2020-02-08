### TimerManager

Start your timers from whatever class you want! Even in the non-MonoBehaviour ones   
### Instalation:
* Create an empty GameObject
* Add this script to this Game Object
* From your class invoke the method startTimer(). The parameters are:   
Time   
A method that will be invoked when the timer is finished   

The TimerManager uses ObjectPool for timers, so there is a limit amount of timers. You can change the amount in **Start** method of **TimerManager**  
You can move Timer and ObjectPool to different classes. I added them to the same file for the sake of simplicity for github
