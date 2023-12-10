import React from "react";
import { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router";

const Register = () => {
  const [formData, setFormData] = useState({
    firstname: "",
    lastname: "",
    username: "",
    email: "",
    password: "",
  });
  const [error, setError] = useState("");
  const [responseError, setResponseError] = useState(null);

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
    } else if (formData.password.length < 8) {
      validationErrors.password =
        "Password should be at least 8 characters long!";
    }
    if (!formData.firstname.trim()) {
      validationErrors.firstname = "First Name is required!";
    }
    if (!formData.lastname.trim()) {
      validationErrors.lastname = "Last Name is required!";
    }
    if (!formData.email.trim()) {
      validationErrors.email = "Email is required!";
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      validationErrors.email = "Email is not valid!";
    }
    setError(validationErrors);
    if (Object.keys(validationErrors).length !== 0) {
      return;
    }

    const formDataToSend = new FormData();
    formDataToSend.append("username", formData.username);
    formDataToSend.append("firstname", formData.firstname);
    formDataToSend.append("lastname", formData.lastname);
    formDataToSend.append("email", formData.email);
    formDataToSend.append("password", formData.password);

    try {
      const response = await axios.post("http://localhost:5083/api/users/register", formDataToSend, {
        headers: {
          "Content-Type": "application/json",
        },
      });
      if (response.data.isSuccess) {
        navigate("/login");
      } else {
        setResponseError(response.data.errorMessages[0]);
      }
    } catch (error) {
      if (error?.response?.data?.title) {
        alert(error?.response?.data?.title);
      } else if (error?.response?.statusText) {
        alert(error?.response?.statusText);
      } else {
        alert(error?.message);
      }
      return;
    }
  };

  return (
    <div className="login-container">
      <div className="form-title">
        <h1>Register</h1>
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
            id="username"
            autoComplete="off"
          />
        </div>
        {error?.username && <span className="error">{error.username}</span>}
        <div className="form-field">
          <label htmlFor="firstname">First Name</label>
          <input
            type="text"
            name="firstname"
            onChange={handleChange}
            value={formData.firstname}
            id="firstname"
          />
        </div>
        {error?.firstname && <span className="error">{error.firstname}</span>}
        <div className="form-field">
          <label htmlFor="lastname">Last Name</label>
          <input
            type="text"
            name="lastname"
            onChange={handleChange}
            value={formData.lastname}
            id="lastname"
          />
        </div>
        {error?.lastname && <span className="error">{error.lastname}</span>}
        <div className="form-field">
          <label htmlFor="email">Email</label>
          <input
            type="text"
            name="email"
            onChange={handleChange}
            value={formData.email}
            id="email"
            autoComplete="off"
          />
        </div>
        {error?.email && <span className="error">{error.email}</span>}
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
          <button>Register</button>
        </div>
      </form>
      <div className="login-page-no-account">
        <p onClick={() => navigate("/login")}>Already registered? Login!</p>
      </div>
    </div>
  );
};

export default Register;
