
import axios from 'axios';

axios.defaults.baseURL="http://localhost:5220"
export default {
  getTasks: async () => {
    const result = await axios.get(`/items`);
    console.log(result, "get all");

    return result.data;
  },

  addTask: async (name) => {
    console.log('addTask', name);
    const result = await axios.post(`/items`, { id: 0, name: name, isComplete: false });
    console.log(result, "add");

    return result.data;
  },

  setCompleted: async (id) => {
    console.log('setCompleted', { id });
    const result = await axios.put(`/items/${id}`);
    console.log(result, "update");
    return {};
  },

  deleteTask: async (id) => {
    console.log('deleteTask');
    const result = await axios.delete(`/items/${id}`);
    console.log(result, "delete");

    return result.data;
  }

};
