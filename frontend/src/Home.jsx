import React, { useState } from "react";
import axios from "axios"; 

const Home = () => {
    const [file, setFile] = useState();
    const [email, setEmail] = useState(""); 
    const [uploadStatus, setUploadStatus] = useState("initial"); 
    const [uploadMessage, setUploadMessage] = useState(""); 

    const handleChangeFile = (event) => {
        const selectedFile = event.target.files[0];
        if (selectedFile && selectedFile.name.endsWith(".docx")) {
            setFile(selectedFile);
        } else {
            alert("Please select a .docx  file only.");
        }
    };

    const handleChangeEmail = (event) => {
        setEmail(event.target.value);
    };

    const handleSubmit = async (event) => {
        event.preventDefault();

        if (!file || !email) {
            alert("Please select a file and enter an email address.");
            return;
        }

        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email)) {
            alert("Please enter a valid email address.");
            return;
        }

        const formData = new FormData();
        formData.append("file", file);

        try {
            const response = await axios.post(`https://localhost:7050/Files?emailAddress=${encodeURIComponent(email)}`, formData, {
                headers: { "content-type": "multipart/form-data" },
            });

            console.log(response.data)

            if (response.status === 200) { 
                setUploadStatus("success");
                setUploadMessage("File uploaded successfully!"); 
            }

            console.log(response.data);
        } catch (error) {
            console.error(error);
            setUploadStatus("check");
            alert("check email");
        }
    };

    return (
        <div className="App">
            <form onSubmit={handleSubmit}>
                <h1>React File Upload</h1>
                <input type="file" onChange={handleChangeFile} />
                <input
                    type="email"
                    value={email}
                    onChange={handleChangeEmail}
                    placeholder="Email"
                    required
                />
                <button type="submit">Upload</button>
                {uploadStatus === "success" && <p>{uploadMessage}</p>} 
                {uploadStatus === "check" && <p>check email</p>}
            </form>
        </div>
    );
};

export default Home;
