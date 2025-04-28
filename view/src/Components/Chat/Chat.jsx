import React, { useState, useEffect, useRef } from 'react';
import './Chat.scss'; // Make sure you import the SCSS here

const ChatApp = () => {
  const [messages, setMessages] = useState([]);
  const [input, setInput] = useState('');
  const socket = useRef(null);

  useEffect(() => {
    socket.current = new WebSocket('ws://localhost:3001');

    socket.current.onmessage = (event) => {
      setMessages(prev => [...prev, event.data]);
    };

    return () => {
      if (socket.current) {
        socket.current.close();
      }
    };
  }, []);

  const sendMessage = (e) => {
    e.preventDefault();

    if (input.trim() && socket.current.readyState === WebSocket.OPEN) {
      socket.current.send(input);
      setMessages(prev => [...prev, input]); // <- Important to see own message immediately
    }

    setInput('');
  };

return (
    <div className="chat-container">
        <div className="messages">
            {messages.map((msg, index) => (
                <div className="message" key={index}>
                    {msg}
                </div>
            ))}
        </div>

        <form onSubmit={sendMessage} className="chat-form">
            <input
                type="text"
                value={input}
                onChange={(e) => setInput(e.target.value)}
                placeholder="Type your message..."
                className="chat-input"
            />
            <img src="/Icons/SendButton.jpg" alt="Send" className="send-icon" />
        
        </form>
    </div>
);
};

export default ChatApp;
