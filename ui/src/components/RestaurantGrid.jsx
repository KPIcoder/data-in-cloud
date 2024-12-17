// src/components/RestaurantGrid.js
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

const API_URL = "http://localhost:8080/api/v1/restaurants";

const RestaurantGrid = () => {
  const [restaurants, setRestaurants] = useState([]);
  const [open, setOpen] = useState(false);
  const [formData, setFormData] = useState({
    id: null,
    name: "",
    isOpen: false,
  });

  // Fetch restaurants from backend
  const fetchRestaurants = async () => {
    try {
      const response = await axios.get(API_URL);
      setRestaurants(response.data);
    } catch (error) {
      console.error("Error fetching restaurants:", error);
    }
  };

  useEffect(() => {
    fetchRestaurants();
  }, []);

  const handleOpen = (restaurant) => {
    setFormData(restaurant || { id: null, name: "", isOpen: false });
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
        // Update restaurant
        await axios.put(`${API_URL}/${formData.id}`, formData);
      } else {
        // Add restaurant
        await axios.post(API_URL, formData);
      }
      fetchRestaurants();
    } catch (error) {
      console.error("Error saving restaurant:", error);
    }
    handleClose();
  };

  const handleDelete = async (id) => {
    try {
      await axios.delete(`${API_URL}/${id}`);
      fetchRestaurants();
    } catch (error) {
      console.error("Error deleting restaurant:", error);
    }
  };

  const columns = [
    { field: "name", headerName: "Name", flex: 1 },
    {
      field: "isOpen",
      headerName: "Open",
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
      <h2>Restaurants</h2>
      <Button variant="contained" onClick={() => handleOpen(null)}>
        Add Restaurant
      </Button>
      <div style={{ height: 400, marginTop: 20 }}>
        <DataGrid
          rows={restaurants}
          columns={columns}
          getRowId={(row) => row.id}
        />
      </div>

      <Dialog open={open} onClose={handleClose}>
        <DialogTitle>
          {formData.id ? "Update Restaurant" : "Add Restaurant"}
        </DialogTitle>
        <DialogContent>
          <TextField
            label="Name"
            name="name"
            fullWidth
            value={formData.name}
            onChange={handleChange}
          />
          <Checkbox
            checked={formData.isOpen}
            onChange={(e) =>
              setFormData({ ...formData, isOpen: e.target.checked })
            }
          />
          Open
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>Cancel</Button>
          <Button onClick={handleSave}>Save</Button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default RestaurantGrid;
