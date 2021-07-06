namespace KawashimaBattleRoyaleCommon {
    /// <summary>
    ///     A list of all packets to be sent to and from clients
    /// </summary>
    public enum PacketType : uint {
        /// <summary>
        ///     Sent to the server on login
        /// </summary>
        LEARNER_LOGIN,
        /// <summary>
        ///     Sent to the server on logout
        /// </summary>
        LEARNER_LOGOUT,
        /// <summary>
        ///     Sent to the server when the client joins the game
        /// </summary>
        LEARNER_JOIN_GAME,
        /// <summary>
        ///     Sent to the server when the client leaves the game
        /// </summary>
        LEARNER_LEAVE_GAME,
        /// <summary>
        ///     Sent to the server to request the current game state
        /// </summary>
        LEARNER_GET_GAME_DATA,
        /// <summary>
        ///     Sent to the server to indicate that the client is sending a question to another client
        /// </summary>
        LEARNER_SEND_QUESTION,

        /// <summary>
        ///     Sent to the client to say a new player has logged in
        /// </summary>
        DRKAWASHIMA_LEARNER_LOGIN,
        /// <summary>
        ///     Sent to the client to say a player has logged out
        /// </summary>
        DRKAWASHIMA_LEARNER_LOGOUT,
        /// <summary>
        ///     Sent to the client to update their game state
        /// </summary>
        DRKARASHIMA_GAME_DATA,
        /// <summary>
        ///     Sent to the client to say a new player has joined the game
        /// </summary>
        DRKAWASHIMA_LEARNER_JOIN,
        /// <summary>
        ///     Sent to the client to say a new player has left the game (but still online)
        /// </summary>
        DRKAWASHIMA_LEARNER_LEAVE,
        /// <summary>
        ///     Sent to a client to tell them that they recieved a question
        /// </summary>
        DRKAWASHIMA_SEND_QUESTION
	}

	public enum ProblemTypes {
		ADDITION   = 1,
		SUBTRACT   = 2,
		MULTIPLY   = 3,
		DIVIDE     = 4,
		MODULO     = 5,
		SQUAREROOT = 6,
		POWER      = 7,
		FACTORIAL  = 8
	}
}
