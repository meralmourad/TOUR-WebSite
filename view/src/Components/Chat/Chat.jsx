import React, { useEffect, useState, useRef } from "react";

const ChatBar = () => {
  const [messages, setMessages] = useState([]);
  const [input, setInput] = useState("");
  const ws = useRef(null);

  useEffect(() => {
    // Connect to WebSocket server
    ws.current = new WebSocket("ws://localhost:3000"); // change URL as needed

    ws.current.onopen = () => {
      console.log("WebSocket connection established");
    };

    ws.current.onmessage = (event) => {
      const message = event.data;
      setMessages((prev) => [...prev, { text: message, fromServer: true }]);
    };

    ws.current.onclose = () => {
      console.log("WebSocket connection closed");
    };

    return () => {
      ws.current.close();
    };
  }, []);

  const sendMessage = () => {
    if (input.trim() === "") return;
    ws.current.send(input);
    setMessages((prev) => [...prev, input]);
    setInput("");
  };

  const handleKeyPress = (e) => {
    if (e.key === "Enter") {
      sendMessage();
    }
  };

  return (
    <div className="chat-bar">
      <div className="chat-messages">
        {messages.map((msg, index) => (
          <div
            key={index}
            className="message"
            style={{
              backgroundColor: msg.fromServer ? "#e0f7fa" : "#c8e6c9",
              alignSelf: msg.fromServer ? "flex-start" : "flex-end",
              marginLeft: msg.fromServer ? "0" : "auto",
              marginRight: msg.fromServer ? "auto" : "0",
              maxWidth: "80%",
            }}
          >
            {msg.text}
          </div>
        ))}
      </div>
      <div className="chat-input">
        <input
          type="text"
          placeholder="Type your message..."
          value={input}
          onChange={(e) => setInput(e.target.value)}
          onKeyPress={handleKeyPress}
        />
        <button className="send-button" onClick={sendMessage}>
          {/* <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24">
            <path d="M2 21l21-9L2 3v7l15 2-15 2v7z" /> */}
          {/* </svg> */}
        </button>
      </div>
    </div>
  );
};

export default ChatBar;
