import "./login.css";
import axios from "axios";
import React, { useState } from "react";
import { useDispatch } from "react-redux";
import { login } from "../store";
import { useNavigate } from "react-router";

const Login = () => {
  const [formData, setFormData] = useState({
    username: "",
    password: "",
  });
  const [error, setError] = useState("");
  const [responseError, setResponseError] = useState(null);

  const dispatch = useDispatch();
  const navigate = useNavigate();

  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormData((prevFormData) => ({
      ...prevFormData,
      [name]: value,
    }));
  };
  const handleSubmit = async (event) => {
    event.preventDefault();

    const validationErrors = {};

    if (!formData.username.trim()) {
      validationErrors.username = "Username is required!";
    }
    if (!formData.password.trim()) {
      validationErrors.password = "Password is required!";
    }
    setError(validationErrors);
    if (Object.keys(validationErrors).length !== 0) {
      return;
    }
    const formDataToSend = new FormData();
    formDataToSend.append("username", formData.username);
    formDataToSend.append("password", formData.password);

    try {
      const response = await axios.post("http://localhost:5083/api/users/login", formDataToSend, {
        headers: {
          "Content-Type": "application/json",
        },
      });
      if (response?.data?.isSuccess && response?.data?.result?.user != null) {
       
        const user = response.data.result.user;
        dispatch(login(user));
        window.localStorage.setItem("user", JSON.stringify(user));
        navigate("/home");
      }
     
      if (!response?.data?.isSuccess) {
        setResponseError(response.data.errorMessages[0]);
      }
    } catch (error) {
      error?.response?.data?.title ? alert(error.response.data.title) : alert(error.message);
      setError(error);
    }
  };

  const handleToRegister = (e) => {
    navigate("/register");
  };

  return (
    <div className="login-container">
      <div className="form-title">
        <h1>Log In</h1>
        <p>Fill out the Form!</p>
      </div>
      <form onSubmit={handleSubmit} className="login-form">
        <div className="form-field">
          <label htmlFor="username">Username</label>
          <input
            type="text"
            name="username"
            onChange={handleChange}
            value={formData.username}
            autoComplete="off"
            id="username"
          />
        </div>
        {error?.username && <span className="error">{error.username}</span>}
        <div className="form-field">
          <label htmlFor="password">Password</label>
          <input
            type="password"
            name="password"
            onChange={handleChange}
            value={formData.password}
            id="password"
            autoComplete="off"
          />
        </div>
        {error?.password && <span className="error">{error.password}</span>}
        {responseError && <p className="error">{responseError}</p>}
        <div className="form-buttons">
          <button type="button" onClick={() => navigate("/")}>
            Back
          </button>
          <button type="button" onClick={handleSubmit}>
            Login
          </button>
        </div>
      </form>
      <div className="login-page-no-account">
        <p onClick={handleToRegister}>Click here to Register!</p>
      </div>
    </div>
  );
};

export default Login;
