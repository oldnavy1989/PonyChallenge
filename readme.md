### Technology

This is a c# .net core console application, which visualises the maze on console for each pony step. 

###Solution
Contains of two projects:
- PonyApiClient (Class Library) - is a wrapper for http requests;
- PonyChallenge (Console Application) - contains path finding logic and game loop.


### Configuration

Use AppSettings.json to introduce maze settings.

```json
{
	"MazeSettings": {
		"Difficulty": 10,
		"Width": 25,
		"Height": 20,
		"PonyName": "Applejack"
	}
}
```
### How to run
 Run solution in Visual Studio or navigate to bin\Debug\netcoreapp2.1\publish and run .exe file for win x64.

### Implementation details

Pony recalculates the path to the exit on each step trying to avoid the monster.
Path finding method based on A\* algorithm.
If there's no available path pony simply stays on it's place. For some reason (probably a bug) game wouldn't stop if monster steps in the same cell if pony is not moving, which allows pony to avoid the monster and win in any game.

