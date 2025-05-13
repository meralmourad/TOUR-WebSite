import { useState, useRef, useEffect } from "react";
import "./Chat.scss";
import { useSelector } from "react-redux";
import { getMessages, sendMessage } from "../../service/MessageService";

const WS_URL = process.env.REACT_APP_WS_URL;

const Chat = () => {
  const { senderId, receiverId } = useSelector((store) => store.chat);
  const { token } = useSelector((store) => store.info);
  const [messages, setMessages] = useState([]);
  const [input, setInput] = useState("");
  const messagesEndRef = useRef(null);

  const ws = useRef(null);

  useEffect(() => {
    // console.log(`${WS_URL}/${senderId}?token=${token}`);
    ws.current = new WebSocket(`${WS_URL}/${senderId}?token=${token}`);

    ws.current.onopen = () => {
      // console.log("WebSocket connected");
      // ws.current.send(" ");
    };

    ws.current.onmessage = (event) => {
      // console.log("Received:", event.data);
      const message = JSON.parse(event.data);

      if (message.SenderId === receiverId) {
        setMessages([
          ...messages,
          {
            text: message.Content,
            sender: "receiver",
          },
        ]);
      }
    };

    ws.current.onerror = (error) => {
      console.error("WebSocket error:", error);
    };

    ws.current.onclose = () => {
      console.log("WebSocket disconnected");
    };

    return () => {
      ws.current.close();
    };
  }, [token, senderId, messages]);

  useEffect(() => {
    const fetchMessages = async () => {
      try {
        const response = await getMessages(senderId, receiverId);
        setMessages(
          response.map((message) => {
            return {
              text: message.content,
              sender: message.senderId === senderId ? "sender" : "receiver",
            };
          })
        );
      } catch (error) {
        console.error("error in fetch messages, ", error);
      }
    };
    fetchMessages();
  }, [senderId, receiverId]);

  const handleSend = async () => {
    if (input.trim()) {
      try {
        await sendMessage(senderId, receiverId, input);
        setMessages([...messages, { text: input, sender: "sender" }]);
        setInput("");
      } catch (error) {
        console.error(error);
      }
    }
  };

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  return (
    <div className="chat-container">
      <div className="chat-messages">
        {messages.map((message, index) => (
          <div
            key={index}
            className={`chat-bubble ${
              message.sender === "sender" ? "user-bubble" : "bot-bubble"
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
          onKeyDown={(e) => e.key === "Enter" && handleSend()}
        />
        <button className="send-button" onClick={handleSend}>
          ➤
        </button>
      </div>
    </div>
  );
};

export default Chat;
