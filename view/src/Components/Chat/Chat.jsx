import { useState, useRef, useEffect } from 'react';
import './Chat.scss';
import { useSelector } from 'react-redux';

const Chat = () => {
  const { senderId, receiverId } = useSelector((store) => store.chat);
  const [messages, setMessages] = useState([]);
  const [input, setInput] = useState('');
  const messagesEndRef = useRef(null);

  useEffect(() => {
    
  }, [senderId, receiverId])

  const handleSend = () => {
    if (input.trim()) {
      const newMessages = [
        ...messages,
        { text: input, sender: 'user' },
        { text: 'hello, how can i help you ✅', sender: 'bot' },
      ];
      setMessages(newMessages);
      setInput('');
    }
  };

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages]);

  return (
    <div className="chat-container">
      <div className="chat-messages">
        {messages.map((message, index) => (
          <div
            key={index}
            className={`chat-bubble ${
              message.sender === 'user' ? 'user-bubble' : 'bot-bubble'
            }`}
          >
            {message.text}
          </div>
        ))}
        <div ref={messagesEndRef} />
      </div>
      <div className="chat-input-container">
        <input
          type="text"
          className="chat-input"
          value={input}
          onChange={(e) => setInput(e.target.value)}
          placeholder="Type a message..."
          onKeyDown={(e) => e.key === 'Enter' && handleSend()}
        />
        <button className="send-button" onClick={handleSend}>
          ➤
        </button>
      </div>
    </div>
  );
};

export default Chat;
