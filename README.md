# Smart Breaker
 Keeps power running, when away or outside of the grid for long period of time.
 
 Requirements:
 1 Programmable Block.
 1 Sensor
 1 Timer Block
 1 Batter
 
At any given time, all 4 of these blocks must remain on. If the server is set to turn off blocks due to inactivity, any one of these blocks can interrupt the process disabling the script. If the 4 hours is the MAX inactivity time, then setting to 3hr 30min would reset the inactivity timer kepeing the functional blocks running. This has been preset to tblock.TriggerDelay = 10800.

u/inert-
