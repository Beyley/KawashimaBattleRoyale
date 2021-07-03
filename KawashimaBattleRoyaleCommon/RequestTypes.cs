namespace KawashimaBattleRoyaleCommon {
    /// <summary>
    /// A list of all packets to be sent to and from clients
    /// </summary>
    public enum PacketType : uint {
        /// <summary>
        /// Sent to the server on login
        /// </summary>
        LEARNER_LOGIN,
        /// <summary>
        /// Sent to the server on logout
        /// </summary>
        LEARNER_LOGOUT,
        /// <summary>
        /// Sent to the server to request the current game state
        /// </summary>
        LEARNER_GET_GAME_DATA,
        
        /// <summary>
        /// Sent to the client to say a new player has logged in
        /// </summary>
        DRKAWASHIMA_LEARNER_LOGIN,
        /// <summary>
        /// Sent to the client to say a player has logged out
        /// </summary>
        DRKAWASHIMA_LEARNER_LOGOUT,
        /// <summary>
        /// Sent to the client to update their game state
        /// </summary>
        DRKARASHIMA_GAME_DATA,
    }
}
