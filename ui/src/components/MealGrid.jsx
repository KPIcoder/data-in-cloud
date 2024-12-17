// src/components/MealGrid.js
import React, { useState, useEffect } from "react";
import axios from "axios";
import { DataGrid } from "@mui/x-data-grid";
import {
  Button,
  Dialog,
  TextField,
  Checkbox,
  DialogActions,
  DialogContent,
  DialogTitle,
} from "@mui/material";

const API_URL = "http://localhost:8080/api/v1/meals";

const MealGrid = () => {
  const [meals, setMeals] = useState([]);
  const [open, setOpen] = useState(false);
  const [formData, setFormData] = useState({
    id: null,
    name: "",
    price: 1,
    isAvailable: false,
  });

  // Fetch meals from backend
  const fetchMeals = async () => {
    try {
      const response = await axios.get(API_URL);
      setMeals(response.data);
    } catch (error) {
      console.error("Error fetching meals:", error);
    }
  };

  useEffect(() => {
    fetchMeals();
  }, []);

  const handleOpen = (meal) => {
    setFormData(meal || { id: null, name: "", price: 1, isAvailable: false });
    setOpen(true);
  };

  const handleClose = () => setOpen(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSave = async () => {
    try {
      if (formData.id) {
        // Update meal
        await axios.patch(`${API_URL}/${formData.id}`, formData);
      } else {
        // Add meal
        await axios.post(API_URL, formData);
      }
      fetchMeals();
    } catch (error) {
      console.error("Error saving meal:", error);
    }
    handleClose();
  };

  const handleDelete = async (id) => {
    try {
      await axios.delete(`${API_URL}/${id}`);
      fetchMeals();
    } catch (error) {
      console.error("Error deleting meal:", error);
    }
  };

  const columns = [
    { field: "name", headerName: "Name", flex: 1 },
    { field: "price", headerName: "Price", flex: 1 },
    {
      field: "isAvailable",
      headerName: "Available",
      flex: 1,
      renderCell: (params) => (params.value ? "Yes" : "No"),
    },
    {
      field: "actions",
      headerName: "Actions",
      flex: 1,
      renderCell: (params) => (
        <>
          <Button onClick={() => handleOpen(params.row)}>Edit</Button>
          <Button color="error" onClick={() => handleDelete(params.row.id)}>
            Delete
          </Button>
        </>
      ),
    },
  ];

  return (
    <div>
      <h2>Meals</h2>
      <Button variant="contained" onClick={() => handleOpen(null)}>
        Add Meal
      </Button>
      <div style={{ height: 400, marginTop: 20 }}>
        <DataGrid rows={meals} columns={columns} getRowId={(row) => row.id} />
      </div>

      <Dialog open={open} onClose={handleClose}>
        <DialogTitle>{formData.id ? "Update Meal" : "Add Meal"}</DialogTitle>
        <DialogContent>
          <TextField
            label="Name"
            name="name"
            fullWidth
            value={formData.name}
            onChange={handleChange}
          />
          <TextField
            label="Price"
            name="price"
            type="number"
            fullWidth
            value={formData.price}
            onChange={handleChange}
          />
          <Checkbox
            checked={formData.isAvailable}
            onChange={(e) =>
              setFormData({ ...formData, isAvailable: e.target.checked })
            }
          />
          Available
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>Cancel</Button>
          <Button onClick={handleSave}>Save</Button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default MealGrid;
