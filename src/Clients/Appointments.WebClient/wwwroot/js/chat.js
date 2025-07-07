// chat.js

let currentChatRoomId = null;
let currentReceiverId = null;


const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7001/chatHub", {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
        // Add JWT token to headers
        accessTokenFactory: async () => { return jwtToken; }
    })
    .build();

connection.on("ReceiveMessage", (chatRoomId, senderId, receiverId, message, timestamp) => {
    if (chatRoomId === currentChatRoomId) {
        const messagesList = document.getElementById("messagesList");
        const li = document.createElement("li");
        li.textContent = `[${new Date(timestamp).toLocaleTimeString()}] From ${senderId} to ${receiverId}: ${message}`;
        messagesList.appendChild(li);
        messagesList.scrollTop = messagesList.scrollHeight; // Scroll to bottom
    }
});

document.getElementById("sendButton").addEventListener("click", async (event) => {
    const message = document.getElementById("messageInput").value;

    if (currentChatRoomId && currentReceiverId && message) {
        try {
            await connection.invoke("SendMessage", currentChatRoomId, currentReceiverId, message);
            document.getElementById("messageInput").value = "";
        } catch (err) {
            console.error(err);
        }
    }
    event.preventDefault();
});

async function startSignalR() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.error("SignalR Connection Error: ", err);
        setTimeout(startSignalR, 5000);
    }
}

connection.onclose(async () => {
    console.log("SignalR Connection Closed. Attempting to restart...");
    await startSignalR();
});

startSignalR();

// --- UI Logic ---

document.addEventListener("DOMContentLoaded", () => {
    const userList = document.getElementById("userList");
    const chatRoomList = document.getElementById("chatRoomList");
    const chatWindow = document.getElementById("chatWindow");
    const chatPartnerName = document.getElementById("chatPartnerName");
    const messagesList = document.getElementById("messagesList");

    const setChatButton = document.getElementById("setChatButton");

    setChatButton.addEventListener("click", async () => {
        const manualChatRoomId = document.getElementById("manualChatRoomId").value;
        const manualReceiverId = document.getElementById("manualReceiverId").value;

        if (manualChatRoomId && manualReceiverId) {
            currentChatRoomId = manualChatRoomId;
            currentReceiverId = manualReceiverId;
            chatPartnerName.textContent = `Chat with ${manualReceiverId} (Manual)`;
            chatWindow.style.display = "block";
            messagesList.innerHTML = ""; // Clear previous messages
            await loadChatMessages(currentChatRoomId);
        } else {
            alert("Please enter both Chat Room ID and Receiver User ID.");
        }
    });

    // Handle user selection to start new chat
    userList.addEventListener("click", async (event) => {
        const target = event.target.closest("li");
        if (target && target.dataset.userId) {
            const selectedUserId = target.dataset.userId;
            const selectedUserName = target.dataset.userName;

            // Try to create or get chat room
            try {
                const response = await fetch("https://localhost:7001/api/v1/Chat/create-room", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `Bearer ${jwtToken}`
                    },
                    body: JSON.stringify({ user1Id: currentUserId, user2Id: selectedUserId })
                });

                if (response.ok) {
                    currentChatRoomId = await response.json();
                    currentReceiverId = selectedUserId;
                    chatPartnerName.textContent = `Chat with ${selectedUserName}`;
                    chatWindow.style.display = "block";
                    messagesList.innerHTML = ""; // Clear previous messages
                    await loadChatMessages(currentChatRoomId);
                } else {
                    console.error("Failed to create/get chat room:", response.status, response.statusText);
                }
            } catch (error) {
                console.error("Error creating/getting chat room:", error);
            }
        }
    });

    // Handle existing chat room selection
    chatRoomList.addEventListener("click", async (event) => {
        const target = event.target.closest("li");
        if (target && target.dataset.roomId) {
            currentChatRoomId = target.dataset.roomId;
            // Determine chat partner name from the list item text
            chatPartnerName.textContent = `Chat with ${target.textContent.trim()}`;
            chatWindow.style.display = "block";
            messagesList.innerHTML = ""; // Clear previous messages
            await loadChatMessages(currentChatRoomId);

            // Find the receiver ID for this chat room
            const chatRoomsData = JSON.parse(document.getElementById('chatRoomsData').textContent);
            const selectedRoom = chatRoomsData.find(room => room.id === currentChatRoomId);
            if (selectedRoom) {
                currentReceiverId = (selectedRoom.user1.id === currentUserId) ? selectedRoom.user2.id : selectedRoom.user1.id;
            }
        }
    });

    async function loadChatMessages(chatRoomId) {
        try {
            debugger;
            const response = await fetch(`https://localhost:7001/api/v1/Chat/messages/${chatRoomId}`, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${jwtToken}`
                },
                //body: JSON.stringify({ user1Id: currentUserId, user2Id: currentReceiverId }),
                credentials: 'include'
            });
            if (response.ok) {
                const messages = await response.json();
                messagesList.innerHTML = ""; // Clear existing messages
                messages.forEach(msg => {
                    const li = document.createElement("li");
                    li.textContent = `[${new Date(msg.timestamp).toLocaleTimeString()}] From ${msg.sender.userName} to ${msg.receiver.userName}: ${msg.message}`;
                    messagesList.appendChild(li);
                });
                messagesList.scrollTop = messagesList.scrollHeight; // Scroll to bottom
            } else {
                console.error("Failed to load chat messages:", response.status, response.statusText);
            }
        } catch (error) {
            console.error("Error loading chat messages:", error);
        }
    }
});