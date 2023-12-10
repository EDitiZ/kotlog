import "./login.css";
import axios from "axios";
import React, { useState } from "react";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router";

const AddTransaction = () => {
  const user = useSelector((state) => state.user.user);
  console.log(user.userName);
  const [formData, setFormData] = useState({
    category: "",
    amount: 0,
    date: "",
  });

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

    const formDataToSend = new FormData();
    formDataToSend.append("user", user.userName);
    formDataToSend.append("category", formData.category);
    formDataToSend.append("amount", formData.amount);
    formDataToSend.append("date", formData.date);

    try {
      const response = await axios.post(
        "http://localhost:5083/api/finances/add-transaction",
        formDataToSend,
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="login-container">
      <div className="form-title">
        <h1>Add Transaction</h1>
        <p>Fill out the Form!</p>
      </div>
      <form onSubmit={handleSubmit} className="login-form">
        <div className="form-field">
          <label htmlFor="category">Category</label>
          <input
            type="text"
            name="category"
            onChange={handleChange}
            value={formData.category}
            autoComplete="off"
            id="category"
          />
        </div>

        <div className="form-field">
          <label htmlFor="amount">Amount</label>
          <input
            type="number"
            name="amount"
            onChange={handleChange}
            value={formData.amount}
            id="amount"
            autoComplete="off"
          />
        </div>

        <div className="form-field">
          <label htmlFor="date">Date</label>
          <input
            type="text"
            name="date"
            onChange={handleChange}
            value={formData.date}
            id="date"
            autoComplete="off"
          />
        </div>

        <div className="form-buttons">
          <button type="button" onClick={() => navigate("/home")}>
            Back
          </button>
          <button type="button" onClick={handleSubmit}>
            Add
          </button>
        </div>
      </form>
    </div>
  );
};

export default AddTransaction;
