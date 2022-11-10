namespace OldBattleships
{
    class Program
    {
        /// <summary>
        /// Draws the battlefield. First array is enemy field, second is friendly field.
        /// </summary>
        /// <param name="field1"></param>
        /// <param name="field2"></param>
        static void FieldDrawer(int[,] field1, int[,] field2)
        {
            Console.WriteLine("    A B C D E F G H I J  ");
            Console.WriteLine("  ##|#|#|#|#|#|#|#|#|#|##");

            // Go through each value in the list and draw them on the screen.
            for (int y = 0; y < 10; y++)
            {
                Console.Write(y + " #");
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                for (int x = 0; x < 10; x++)
                {
                    // Check which symbol to be drawn.
                    switch (field1[x, y])
                    {
                        case 0:
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write(" ");
                            Console.Write("~");
                            break;
                        case 1:
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write(" ");
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.Write("O");
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            break;
                        case 2:
                            Console.Write(" ");
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write("X");
                            break;
                        case 3:
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write(" ");
                            Console.Write("~");
                            break;
                        case 4:
                            Console.Write(" ");
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write("■");
                            break;
                    }
                }
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.Write(" ");
                Console.ResetColor();
                Console.Write("#");
                Console.WriteLine();
            }

            Console.WriteLine("  #######################");
            Console.WriteLine("    A B C D E F G H I J  ");
            Console.WriteLine("  ##|#|#|#|#|#|#|#|#|#|##");

            // Go through each value in the list and draw them on the screen.
            for (int y = 0; y < 10; y++)
            {
                Console.Write(y + " #");
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                for (int x = 0; x < 10; x++)
                {
                    // Check which symbol to be drawn.
                    switch (field2[x, y])
                    {
                        case 0:
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write(" ");
                            Console.Write("~");
                            break;
                        case 1:
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write(" ");
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.Write("O");
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            break;
                        case 2:
                            Console.Write(" ");
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write("X");
                            break;
                        case 3:
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write(" ");
                            Console.Write("■");
                            break;
                        case 4:
                            Console.Write(" ");
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write("■");
                            break;
                    }
                }
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.Write(" ");
                Console.ResetColor();
                Console.Write("#");
                Console.WriteLine();
            }
            Console.WriteLine("  #######################");
        }



        /// <summary>
        /// Ask for coordinates in the format a7. Supports a-j and 0-9.
        /// </summary>
        /// <returns>
        /// int in range of 0-99
        /// </returns>
        static Tuple<int, int> ConvertCoords()
        {
            // Loop until success.
            while (true)
            {
                // Try to convert the coords into a int, if an error occurs, ask for a new set of coordinates.
                try
                {
                    string coords = Console.ReadLine().ToLower();
                    char xCoords2 = Char.Parse(coords.Substring(0,1));
                    int x = xCoords2 % 32;
                    if (x > 10) {Console.WriteLine("ERROR: INVALID COORDINATE SYNTAX");Console.WriteLine("Please try again!");continue; }
                    return Tuple.Create(x-1,Int32.Parse(coords.Substring(1, 1)));
                }
                catch
                {
                    Console.WriteLine("ERROR: INVALID COORDINATE SYNTAX");
                    Console.WriteLine("Please try again!");
                }
            }
        }



        /// <summary>
        /// Ask for direction and check collisions.
        /// </summary>
        /// <param name="xCoord">Suggested x coordinate</param>
        /// <param name="yCoord">Suggested y coordinate</param>
        /// <param name="ship">Current ship lenght</param>
        /// <param name="field">Current game field</param>
        /// <param name="dirNr">Current chosen direction</param>
        /// <returns></returns>
        static Tuple<int, int, bool> CheckDirCollisions(int xCoord, int yCoord, int ship, int[,] field, int dirNr)
        {
            // Main direction testing loop.
            while (true)
            {
                // Set offsets depending on desired direction. dirNr, 0 = North, 1 = East, 2 = South, 3 = West
                int xCoordOffset = 0;
                int yCoordOffset = 0;
                switch (dirNr)
                {
                    case 0: yCoordOffset = -1; break;
                    case 1: xCoordOffset = 1; break;
                    case 2: yCoordOffset = 1; break;
                    case 3: xCoordOffset = -1; break;
                }


                // Check if the ship fits in the location without going out of bounds
                //___________________________________________________________________
                // Create needed variables for following logic.
                int xChecking = xCoord;
                int yChecking = yCoord;
                // Cycle through the coordinates the ship would be at.
                for (int i = 0; i < ship; i++)
                { 
                    xChecking += xCoordOffset;
                    yChecking += yCoordOffset;
                    // Check if the current coordinates are outside the map
                    if (xChecking is < 0 or > 9 || yChecking is < 0 or > 9)
                    {
                        // If they are, end the method and output a false.
                        return Tuple.Create(0, 0, false);
                    }
                }


                // Scan a rectangle around the ship area for other ships. Return true if nothing is found, otherwise false.
                //_________________________________________________________________________________________________________
                // Set starting coordinates for the scan.
                int xCheckingStartPosition = xCoord + (xCoordOffset * -1)+ (yCoordOffset * -1);
                int yCheckingStartPosition = yCoord + (xCoordOffset * -1)+ (yCoordOffset * -1);
                xChecking = xCheckingStartPosition;
                yChecking = yCheckingStartPosition;

                // Start the scan. First for loop decided between the three columns of the rectangle.
                for (int i = 0; i < 3; i++)
                {
                    // Second for loop goes through each coordinate point in the rectangle.
                    for (int i2 = 0; i2 < ship + 2; i2++)
                    {
                        // First make sure the coordinate is inside the map, to prevent a indexOutOfRange error.
                        if (xChecking is >= 0 and <= 9 && yChecking is >= 0 and <= 9)
                        {
                            #region Debug
                            //Console.Write($"Checking {xChecking}.{yChecking}...");
                            #endregion
                            // Open that location in the field array and see if a ship occupies the point.
                            if (field[xChecking, yChecking] != 0)
                            {
                                #region Debug
                                //Console.WriteLine("TRUE");
                                #endregion
                                // If it does, end the method and output a false.
                                return Tuple.Create(0, 0, false);
                            }
                            #region Debug
                            //else {Console.WriteLine("FALSE");}
                            #endregion
                        }
                        // Increase to view the next point in the next loop iteration.
                        xChecking += xCoordOffset;
                        yChecking += yCoordOffset;
                    }

                    // Switch to the next rectangle column, also reset the checking variables back to the new start position.
                    xCheckingStartPosition += yCoordOffset;
                    yCheckingStartPosition += xCoordOffset;
                    xChecking = xCheckingStartPosition;
                    yChecking = yCheckingStartPosition;
                }

                // If no ship was found, then the loop wont end and this will then return a true together with the offsets needed to place the ship.
                return Tuple.Create(xCoordOffset, yCoordOffset, true);
            }
        }



        /// <summary>
        /// Takes a yes or no from the player and returns a bool.
        /// </summary>
        /// <returns></returns>
        static bool YesNoInput()
        {
            while (true)
            {
                try
                {
                    string temp = Console.ReadLine().ToLower();
                    if (temp == "yes" || temp == "y")
                    {
                        return true;
                    }
                    else if (temp == "no" || temp == "n")
                    {
                        return false;
                    }
                }
                catch
                {
                    Console.WriteLine("Please enter Yes or No.");
                }
            }
        }



        /// <summary>
        /// Shoot the target coordinates and check if it is a hit and whether the ship has been sunk. Check that is haven't been tried before passing coords to this method. Also make sure to write the new values to the arrays.
        /// </summary>
        /// <param name="xCoord"></param>
        /// <param name="yCoord"></param>
        /// <param name="field"></param>
        /// <returns>Bool 1 = hit | Bool 2 = ship sunk</returns>
        static Tuple<bool, bool, string[][]> ShootCoordinates(int xCoord, int yCoord, int[,] field, string [][] TargetShips)
        {
            // Check if it is a miss.
            if (field[xCoord, yCoord] == 0)
            {
                // Notify that it is a miss.
                return Tuple.Create(false, false, TargetShips);
            }
            // Check if a ship is at the coordinates.
            if (field[xCoord, yCoord] == 3)
            {
                // Go through the list of ships.
                for (int i = 0; i < 5; i++)
                {
                    int shipPart = 0;

                    // Go through each part of the ship checking if any has been hit.
                    foreach (string str in TargetShips[i])
                    {
                        if (Convert.ToString(xCoord) + Convert.ToString(yCoord) == str)
                        {
                            // Mark the point as a hit ship.
                            field[xCoord, yCoord] = 2;

                            // If a hit has occured, mark that part as hit and scan the rest of the ship. If all parts are hit the ship is sunk.
                            TargetShips[i][shipPart] = "X";

                            // The result of this will only be true if each part has a "X" marking a hit, otherwise it will return false.
                            bool isSunk = true;
                            foreach (string str2 in TargetShips[i])
                            {
                                if (str2 != "X")
                                {
                                    isSunk = false;
                                }
                            }

                            if (isSunk)
                            {
                                // Return that a ship has been hit and destroyed.
                                return Tuple.Create(true, true, TargetShips);
                            }
                            else
                            {
                                // Return that a ship was hit but not destroyed.
                                return Tuple.Create(true, false, TargetShips);
                            }
                        }

                        // change to the next part of the ship.
                        shipPart++;
                    }
                }
            }
            return Tuple.Create(false, false, TargetShips);
        }



        /// <summary>
        /// Main game loop.
        /// </summary>
        static void Main()
        {
            // Main loop TODO

            // First display the playing field.
            // Ask the player to place their ships.
            //   When doing so ask for the starting position of the ship, then direction. Check if compatible, then place
            //   Ships needs one tile of free space around them.

            // Todo list:
            // 2. Start making logic for placing ships
            // 3. Make logic to register hits and victory.
            // 4. Make logic to randomly place ships for the ai
            // 5. Look up strategies for the ai to follow.


            // Generate playing field arrays.
            //_______________________________
            int[,] field1 = new int[10, 10];
            int[,] field2 = new int[10, 10];
            for (int xLevel = 0; xLevel < 10; xLevel++)
            {
                for (int yLevel = 0; yLevel < 10; yLevel++)
                {
                    field1[xLevel, yLevel] = 0;
                    field2[xLevel, yLevel] = 0;
                }
            }


            // Make a list of placeable ships.
            int[] ships = {5, 4, 3, 2, 2,};

            // Prepare arrays of arrays for the ships location
            string[][] playerShips = new string[5][];
            string[][] aiShips = new string[5][];

            // Keep the scores of the player and ai.
            int playerShipsSunk = 0;
            int aiShipsSunk = 0;
            int shipNr = 0;


            Console.WriteLine("Do you want to place your ships yourself? Y-es or N-o?");
            bool dontAutoPlace = YesNoInput();


            // Let the player place their ships in desired locations.
            //_______________________________________________________
            if (dontAutoPlace)
            {
                // Go through the list and let the player place one ship at a time.
                foreach (int ship in ships)
                {
                    FieldDrawer(field1, field2);

                    Console.WriteLine("Ship " + ship);
                    Console.WriteLine("Please enter coordinates: ");

                    // Get coordinates and check if the space is already occupied
                    var (xCoord, yCoord) = ConvertCoords();
                    if (field2[xCoord, yCoord] == 0)
                    {
                        Console.WriteLine("COORDINATES ACCEPTED!");
                        Console.WriteLine("Please enter direction: N-E-S-W allowed. ");

                        while (true)
                        {
                            int dirNr = 5;
                            try
                            {
                                var dir = Console.ReadLine().ToLower().Substring(0, 1);

                                switch (dir)
                                {
                                    case "n":
                                        dirNr = 0;
                                        break;
                                    case "e":
                                        dirNr = 1;
                                        break;
                                    case "s":
                                        dirNr = 2;
                                        break;
                                    case "w":
                                        dirNr = 3;
                                        break;
                                    default:
                                        Console.WriteLine("ERROR: INVALID COORDINATE SYNTAX");
                                        Console.WriteLine("Please try again!");
                                        continue;
                                }

                                #region Debug

                                //Console.WriteLine("DIRECTION ACCEPTED!");
                                ////Console.WriteLine(dirNr);

                                #endregion

                                (int xCoordOffset, int yCoordOffset, bool success) =
                                    CheckDirCollisions(xCoord, yCoord, ship, field2, dirNr);
                                if (success)
                                {
                                    // Create array item with ship coordinates to determine when sunk
                                    playerShips[shipNr] = new string[ship];

                                    // Place ship
                                    for (int i = 0; i < ship; i++)
                                    {
                                        // Add the ships part to the list of parts for that ship.
                                        playerShips[shipNr][i] = Convert.ToString(xCoord) + Convert.ToString(yCoord);

                                        field2[xCoord, yCoord] = 3;
                                        Console.WriteLine($"Placing ship at x={xCoord}, y={yCoord}");
                                        xCoord += xCoordOffset;
                                        yCoord += yCoordOffset;
                                    }

                                    shipNr++;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("INVALID DIRECTION! Ship can not be next to another ship!");
                                    Console.WriteLine("Please try again!");
                                }
                            }
                            catch (NullReferenceException)
                            {
                                Console.WriteLine("Please enter a value.");
                            }
                        }

                        FieldDrawer(field1, field2);

                    }
                    else
                    {
                        Console.WriteLine("Space already occupied!");
                        Console.WriteLine("Please try again!");
                    }
                }
            }


            // Auto place the ships in random locations.
            //__________________________________________
            else
            {
                bool playerNotSatisfied = true;
                while (playerNotSatisfied)
                {
                    // Reset the playing field to allow for new ships to be placed.
                    for (int xLevel = 0; xLevel < 10; xLevel++)
                    {
                        for (int yLevel = 0; yLevel < 10; yLevel++)
                        {
                            field1[xLevel, yLevel] = 0;
                            field2[xLevel, yLevel] = 0;
                        }
                    }
                    shipNr = 0;
                    Array.Clear(playerShips, 0, 5);
                    // Create random ships for the player
                    foreach (int ship in ships)
                    {
                        bool success = false;
                        Random rnd = new Random();
                        // Pick a random coordinate.
                        do
                        {
                            int xCoord = rnd.Next(0, 10);
                            int yCoord = rnd.Next(0, 10);

                            #region Debug

                            Console.Write($"Testing {xCoord}.{yCoord}... ");

                            #endregion

                            if (field2[xCoord, yCoord] == 0)
                            {
                                #region Debug

                                Console.WriteLine($"Success!");

                                #endregion

                                int xCoordOffset = 0;
                                int yCoordOffset = 0;
                                int i = 0;

                                // Make the direction test start from random positions to prevent favour of north.
                                int dirOffset = rnd.Next(0, 5);
                                while (success == false && i < 4)
                                {
                                    // Apply offset
                                    int dirNr = i + dirOffset;
                                    // if result is larger than 3 then remove 4. This keeps the value in the 0 - 3 range.
                                    if (dirNr > 3)
                                    {
                                        dirNr -= 4;
                                    }

                                    #region Debug

                                    Console.WriteLine($"Testing dir: {dirNr}");

                                    #endregion

                                    (xCoordOffset, yCoordOffset, success) =
                                        CheckDirCollisions(xCoord, yCoord, ship, field2, dirNr);

                                    #region Debug

                                    Console.WriteLine($"Success = {success}");

                                    #endregion

                                    i++;
                                }

                                if (success)
                                {
                                    // Create array item with ship coordinates to determine when sunk
                                    playerShips[shipNr] = new string[ship];

                                    // Place ship
                                    for (i = 0; i < ship; i++)
                                    {
                                        Console.WriteLine($"shipNr = {shipNr} i = {i}");
                                        // Add the ships part to the list of parts for that ship.
                                        playerShips[shipNr][i] = Convert.ToString(xCoord) + Convert.ToString(yCoord);

                                        field2[xCoord, yCoord] = 3;
                                        Console.WriteLine($"Placing ship at x={xCoord}, y={yCoord}");
                                        xCoord += xCoordOffset;
                                        yCoord += yCoordOffset;
                                    }
                                    shipNr++;
                                }
                            }

                            #region Debug

                            else
                            {
                                Console.WriteLine($"Fail!");
                            }

                            #endregion

                        } while (!success);
                    }
                    FieldDrawer(field1, field2);

                    Console.WriteLine("Do you want to randomize ship locations again? Y-es or N-o");
                    playerNotSatisfied = YesNoInput();
                }
            }


            // Randomly place the ships for the ai.
            //_____________________________________
            shipNr = 0;
            foreach (int aiShip in ships)
            {
                bool success = false;
                Random rnd = new Random();
                // Pick a random coordinate.
                do
                {
                    int xCoord = rnd.Next(0, 10);
                    int yCoord = rnd.Next(0, 10);

                    #region Debug

                    Console.Write($"Testing {xCoord}.{yCoord}... ");

                    #endregion

                    if (field1[xCoord, yCoord] == 0)
                    {
                        #region Debug

                        Console.WriteLine($"Success!");

                        #endregion

                        int xCoordOffset = 0;
                        int yCoordOffset = 0;
                        int i = 0;

                        // Make the direction test start from random positions to prevent favour of north.
                        int dirOffset = rnd.Next(0, 5);
                        while (success == false && i < 4)
                        {
                            // Apply offset
                            int dirNr = i + dirOffset;
                            // if result is larger than 3 then remove 4. This keeps the value in the 0 - 3 range.
                            if (dirNr > 3)
                            {
                                dirNr -= 4;
                            }

                            #region Debug

                            Console.WriteLine($"Testing dir: {dirNr}");

                            #endregion

                            (xCoordOffset, yCoordOffset, success) =
                                CheckDirCollisions(xCoord, yCoord, aiShip, field1, dirNr);

                            #region Debug

                            Console.WriteLine($"Success = {success}");

                            #endregion

                            i++;
                        }

                        if (success)
                        {
                            // Create array item with ship coordinates to determine when sunk
                            aiShips[shipNr] = new string[aiShip];

                            // Place ship
                            for (i = 0; i < aiShip; i++)
                            {
                                Console.WriteLine($"shipNr = {shipNr} i = {i}");
                                // Add the ships part to the list of parts for that ship.
                                aiShips[shipNr][i] = Convert.ToString(xCoord) + Convert.ToString(yCoord);

                                field1[xCoord, yCoord] = 3;
                                Console.WriteLine($"Placing ship at x={xCoord}, y={yCoord}");
                                xCoord += xCoordOffset;
                                yCoord += yCoordOffset;
                            }
                            shipNr++;
                        }
                    }

                    #region Debug

                    else
                    {
                        Console.WriteLine($"Fail!");
                    }

                    #endregion

                } while (!success);

                FieldDrawer(field1, field2);
            }






            // Main game loop
            //__________________________________________________________________
            string delayedMessage = "";
            var gameOver = false;
            int[] aiHitCoordinates = new int[2];
            bool aiHitWithOutSunk = false;
            bool aiSearchVertical = false;
            bool aiSearchHorizontal = false;
            bool aiSearchNegative = false;

            while (!gameOver) 
            {

                // Player turn.
                //_____________
                while (true)
                {
                    // Ask player for firing target.
                    Console.WriteLine("Please enter firing target. Format: A-J 0-9");
                    var (xCoord, yCoord) = ConvertCoords();

                    // Check if the selected location haven't been targeted before.
                    if (field2[xCoord, yCoord] != 1 || field2[xCoord, yCoord] != 2)
                    {

                        // Check if it is a miss.
                        if (field1[xCoord, yCoord] == 0)
                        {
                            // Set the array to mark a miss.
                            field1[xCoord, yCoord] = 1;
                            // Notify that the player missed.
                            //FieldDrawer(field1,field2);
                            delayedMessage = "Miss!";
                            // End the loop and let the ai play.
                            break;
                        }
                        else
                        {
                            // Check if a ship is at the coordinates.
                            if (field1[xCoord, yCoord] == 3)
                            {
                                // Go through the list of ships.
                                for (int i = 0; i < 5; i++)
                                {
                                    int shipPart = 0;

                                    // Go through each part of the ship checking if any has been hit.
                                    foreach (string str in aiShips[i])
                                    {
                                        if (Convert.ToString(xCoord) + Convert.ToString(yCoord) == str)
                                        {
                                            // Mark the point as a hit ship.
                                            field1[xCoord, yCoord] = 2;

                                            // Draw the battlefield and notify the player of their hit.
                                           // FieldDrawer(field1,field2);
                                            delayedMessage = "Hit!";

                                            // If a hit has occured, mark that part as hit and scan the rest of the ship. If all parts are hit the ship is sunk.
                                            aiShips[i][shipPart] = "X";

                                            // The result of this will only be true if each part has a "X" marking a hit, otherwise it will return false.
                                            bool isSunk = true;
                                            foreach (string str2 in aiShips[i])
                                            {
                                                if (str2 != "X")
                                                {
                                                    isSunk = false;
                                                }
                                            }

                                            // Increase the score and alert the player that they sunk a ship.
                                            if (isSunk)
                                            {
                                                aiShipsSunk++;
                                                delayedMessage = "Ship sunk!";
                                            }
                                        }
                                        // change to the next part of the ship.
                                        shipPart++;
                                    }
                                }
                                // Stop the loop to let the ai play.
                                break;
                            }
                            else
                            {
                                FieldDrawer(field1,field2);
                                Console.WriteLine("This point has already been fired upon. Please select new coordinates.");
                                Console.WriteLine("");
                            }
                        }
                        // Check from ship data if the ship has been sunk.
                    }
                    else
                    {
                        FieldDrawer(field1,field2);
                        Console.WriteLine("Target location has already been destroyed. Please select new coordinates.");
                        Console.WriteLine("");
                    }
                }


                // Ai turn.
                //_________
                while (true)
                {
                    Random rnd = new Random();
                    int xCoord = rnd.Next(0, 10);
                    int yCoord = rnd.Next(0, 10);
                    if (aiHitWithOutSunk)
                    {
                        // randomly select a coordinate unless the ai has made a hit recently without sinking the ship.

                        // if no hit has been made, select a random coordinate and shoot. If already targeted, select a new random coordinate.
                        //    if a hit is made on a ship save the coordinates and set aiHitWithoutSunk to true.

                        // if the ai hit a ship recently. Check the coordinates around the hit for tried points.
                        //    if there is another ship part in any direction, save that direction and only try on that axis until ship is sunk. 
                        //    
                        //    if the ship is sunk then reset variables.


                        // Check coordinates around the previous hit.
                        if (aiSearchHorizontal == false && aiSearchVertical == false)
                        {

                            Console.WriteLine("No search direction found.");

                            // Loop until a valid target has been found.
                            while (true)
                            {
                                int xCoordOffset = 0;
                                int yCoordOffset = 0;

                                // Choose a random direction to test.
                                int dirNr = rnd.Next(0, 4);
                                switch (dirNr)
                                {
                                    case 0: yCoordOffset = -1; break;
                                    case 1: xCoordOffset = 1; break;
                                    case 2: yCoordOffset = 1; break;
                                    case 3: xCoordOffset = -1; break;
                                }

                                Console.WriteLine($"Checking dir: {dirNr}");
                                Console.WriteLine($"Checking {aiHitCoordinates[0] + xCoordOffset} and {aiHitCoordinates[1] + yCoordOffset}");
                                if (aiHitCoordinates[0] + xCoordOffset < 0 || aiHitCoordinates[1] + yCoordOffset < 0 || aiHitCoordinates[0] + xCoordOffset > 9 || aiHitCoordinates[1] + yCoordOffset > 9)
                                {
                                    continue;
                                }
                                Console.WriteLine($"It is a: {field2[aiHitCoordinates[0] + xCoordOffset, aiHitCoordinates[1] + yCoordOffset]}");
                                if (field2[aiHitCoordinates[0] + xCoordOffset, aiHitCoordinates[1] + yCoordOffset] is 0 or 3)
                                {
                                    var (shipHit, shipSunk, playerShipsTemp ) = ShootCoordinates(aiHitCoordinates[0] + xCoordOffset, aiHitCoordinates[1] + yCoordOffset, field2, playerShips);
                                    playerShips = playerShipsTemp;
                                    Console.WriteLine(shipHit);
                                    if (shipHit)
                                    {
                                        // Draw the battlefield and print information.
                                        field2[aiHitCoordinates[0] + xCoordOffset, aiHitCoordinates[1] + yCoordOffset] = 2;
                                        FieldDrawer(field1, field2);
                                        Console.WriteLine(delayedMessage);
                                        Console.WriteLine("Bot Hit!");

                                        // Set the axis to search for more ships.
                                        switch (dirNr)
                                        {
                                            case 0: aiSearchVertical = true; break;
                                            case 1: aiSearchHorizontal = true; break;
                                            case 2: aiSearchVertical = true; break;
                                            case 3: aiSearchHorizontal = true; break;
                                        }
                                        // Pick a random starting direction.
                                        if (rnd.NextDouble() > 0.5) { aiSearchNegative = true; }

                                        if (shipSunk)
                                        {
                                            aiHitWithOutSunk = false;
                                            aiSearchHorizontal = false;
                                            aiSearchVertical = false;
                                            playerShipsSunk++;
                                            Console.WriteLine("The bot sunk one of your ships!");
                                        }

                                        break;
                                    }
                                    else
                                    {
                                        // Draw the battlefield and print information.
                                        field2[aiHitCoordinates[0] + xCoordOffset, aiHitCoordinates[1] + yCoordOffset] = 1;
                                        FieldDrawer(field1, field2);
                                        Console.WriteLine(delayedMessage);
                                        Console.WriteLine("Bot Miss!");
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                        else
                        {

                            Console.WriteLine("Search dir found.");
                            int xScanning = 0;
                            int yScanning = 0;
                            // Search for next part to hit.
                            int temp = 0;
                            while (true && temp < 250)
                            {
                                temp++;
                                Console.WriteLine(aiSearchVertical? "Searching vertical!" : "Searching horizontal!");
                                int xCoordOffset = 0;
                                int yCoordOffset = 0;

                                // Depending on the direction start scanning for next hit.
                                if (aiSearchHorizontal)
                                {
                                    if (aiSearchNegative) { xCoordOffset = -1; }
                                    else { xCoordOffset = 1; }
                                }
                                else if (aiSearchVertical)
                                {
                                    if (aiSearchNegative) { yCoordOffset = -1; }
                                    else { yCoordOffset = 1; }
                                }

                                xScanning += xCoordOffset;
                                yScanning += yCoordOffset;
                                if (aiHitCoordinates[1] + yScanning < 0 || aiHitCoordinates[0] + xScanning < 0 || aiHitCoordinates[1] + yScanning > 9 || aiHitCoordinates[0] + xScanning > 9)
                                {
                                    aiSearchNegative = !aiSearchNegative;
                                    continue;
                                }

                                Console.WriteLine($"Checking {aiHitCoordinates[0] + xScanning} and {aiHitCoordinates[1] + yScanning} equals in a {field2[aiHitCoordinates[0] + xScanning, aiHitCoordinates[1] + yScanning]}");

                                if (field2[aiHitCoordinates[0] + xScanning,
                                    aiHitCoordinates[1] + yScanning] is 0 or 3)
                                {
                                    var (shipHit, shipSunk, playerShipsTemp) = ShootCoordinates(aiHitCoordinates[0] + xScanning, aiHitCoordinates[1] + yScanning, field2, playerShips);
                                    playerShips = playerShipsTemp;
                                    // Tell the ai that the targeted ship has been sunk
                                    if (shipSunk)
                                    {
                                        aiHitWithOutSunk = false;
                                        aiSearchHorizontal = false;
                                        aiSearchVertical = false;
                                        aiSearchNegative = false;
                                    }

                                    // Tell the player the ai hit their ship.
                                    if (shipHit)
                                    {
                                        // Draw the battlefield and print information.
                                        field2[aiHitCoordinates[0] + xScanning, aiHitCoordinates[1] + yScanning] = 2;
                                        FieldDrawer(field1, field2);
                                        Console.WriteLine(delayedMessage);
                                        Console.WriteLine("Bot Hit!");
                                        if (shipSunk)
                                        {
                                            aiHitWithOutSunk = false;
                                            aiSearchHorizontal = false;
                                            aiSearchVertical = false;
                                            playerShipsSunk++;
                                            Console.WriteLine("The bot sunk one of your ships!");
                                        }
                                        break;
                                    }
                                    // Tell the player they missed.
                                    else
                                    {
                                        // Draw the battlefield and print information.
                                        field2[aiHitCoordinates[0] + xScanning, aiHitCoordinates[1] + yScanning] = 1;
                                        FieldDrawer(field1, field2);
                                        Console.WriteLine(delayedMessage);
                                        Console.WriteLine("Bot Miss!");
                                        break;
                                    }
                                }
                                else if (field2[aiHitCoordinates[0] + xScanning,
                                    aiHitCoordinates[1] + yScanning] == 1)
                                {
                                    aiSearchNegative = !aiSearchNegative;
                                }
                            }

                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No previous hit detected.");

                        // Check if the randomly selected coordinates have been targeted before.
                        if (field2[xCoord, yCoord] is 0 or 3)
                        {

                            // Check if it is a miss.
                            if (field2[xCoord, yCoord] == 0)
                            {
                                // Set the array to mark a miss.
                                field2[xCoord, yCoord] = 1;
                                // Notify that the bot missed.
                                FieldDrawer(field1, field2);
                                Console.WriteLine(delayedMessage);
                                Console.WriteLine("Bot Miss!");
                                // End the loop and let the player play.
                                break;
                            }
                            else
                            {
                                // Check if a ship is at the coordinates.
                                if (field2[xCoord, yCoord] == 3)
                                {
                                    // Save the hit coordinates if no hit has been performed before.
                                    Console.WriteLine($"Set aiHitCoordinates to {xCoord} and {yCoord}");
                                    aiHitCoordinates[0] = xCoord;
                                    aiHitCoordinates[1] = yCoord;

                                    // Go through the list of ships.
                                    for (int i = 0; i < 5; i++)
                                    {
                                        int shipPart = 0;

                                        // Go through each part of the ship checking if any has been hit.
                                        foreach (string str in playerShips[i])
                                        {
                                            if (Convert.ToString(xCoord) + Convert.ToString(yCoord) == str)
                                            {
                                                // Mark the point as a hit ship.
                                                field2[xCoord, yCoord] = 2;

                                                // Draw the battlefield and notify the player of their hit.
                                                FieldDrawer(field1, field2);
                                                Console.WriteLine(delayedMessage);
                                                Console.WriteLine("Bot Hit!");

                                                // If a hit has occured, mark that part as hit and scan the rest of the ship. If all parts are hit the ship is sunk.
                                                playerShips[i][shipPart] = "X";

                                                // The result of this will only be true if each part has a "X" marking a hit, otherwise it will return false.
                                                bool isSunk = true;
                                                foreach (string str2 in playerShips[i])
                                                {
                                                    if (str2 != "X")
                                                    {
                                                        isSunk = false;
                                                    }
                                                }

                                                // Increase the score and alert the player that they sunk a ship.
                                                if (isSunk)
                                                {
                                                    // Tell the ai that the targeted ship has sunk.
                                                    aiHitWithOutSunk = false;
                                                    aiSearchHorizontal = false;
                                                    aiSearchVertical = false;
                                                    playerShipsSunk++;
                                                    Console.WriteLine("The bot sunk one of your ships!");
                                                }
                                                else
                                                {
                                                    // Make the ai know if it hit earlier and should follow a search pattern.
                                                    aiHitWithOutSunk = true;
                                                }
                                            }

                                            // change to the next part of the ship.
                                            shipPart++;
                                        }
                                    }

                                    // Stop the loop to let the ai play.
                                    break;
                                }
                            }
                        }
                    }
                }

                Console.WriteLine();

                if (aiShipsSunk >= 5)
                {
                    Console.WriteLine("Player wins!");
                    gameOver = true;
                }
                else if (playerShipsSunk >= 5)
                {
                    Console.WriteLine("The ai wins!");
                    gameOver = true;
                }
            }
            Console.WriteLine("");
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}