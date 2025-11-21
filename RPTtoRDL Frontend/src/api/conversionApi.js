import axios from "axios";

const API_BASE_URL = "https://localhost:44364/api/xml/convert";

export const uploadRPTFile = async (file, onUploadProgress) => {
  const formData = new FormData();
  formData.append("file", file);

  return axios.post(API_BASE_URL, formData, {
    responseType: "blob",
    headers: { "Content-Type": "multipart/form-data" },
    onUploadProgress: (event) => {
      if (event.total) {
        const percent = Math.round((event.loaded * 100) / event.total);
        onUploadProgress(percent);
      }
    }
  });
};
