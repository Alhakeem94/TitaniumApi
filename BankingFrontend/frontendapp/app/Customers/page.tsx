"use client";

import { useState } from "react";

import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Dialog } from "@/components/ui/dialog";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";

export default function AddCustomer() {
  const [message, setMessage] = useState("");

  const [formData, setFormData] = useState({
    customerName: "",
    customerDateOfBirth: "",
    customerNationalId: "",
    isMale: true,
    grade: "",
    notes: "",
    customerEmail: "",
    customerPhoenNumber: "",
    adress: "",
  });

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    setFormData((prev) => ({
      ...prev,
      [e.target.name]: e.target.value,
    }));
  };

  const submitCustomer = async (e: React.FormEvent) => {
    e.preventDefault();

    const payload = {
      customerName: formData.customerName,
      customerDateOfBirth: formData.customerDateOfBirth,
      customerNationalId: Number(formData.customerNationalId),
      isMale: formData.isMale,
      grade: Number(formData.grade),
      notes: formData.notes,
      customerEmail: formData.customerEmail,
      customerPhoenNumber: formData.customerPhoenNumber,
      adress: formData.adress,
    };

    try {
      const response = await fetch(
        "http://localhost:5252/api/Customers/AddNewCustomer",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(payload),
        }
      );

      const result = await response.text();

      if (response.ok) {
        setMessage(result);

        setFormData({
          customerName: "",
          customerDateOfBirth: "",
          customerNationalId: "",
          isMale: true,
          grade: "",
          notes: "",
          customerEmail: "",
          customerPhoenNumber: "",
          adress: "",
        });
      } else {
        setMessage(result);
      }
    } catch {
      setMessage("Failed to connect to server.");
    }
  };

  return (
    <div className="container mx-auto max-w-4xl p-6">
      <Card>
        <CardHeader>
          <CardTitle className="text-2xl">
            Add New Customer
          </CardTitle>
        </CardHeader>

        <CardContent>
          <form onSubmit={submitCustomer} className="space-y-6">
            <div className="grid gap-4 md:grid-cols-2">
              <div className="space-y-2">
                <Label htmlFor="customerName">Customer Name</Label>
                <Input
                  id="customerName"
                  name="customerName"
                  value={formData.customerName}
                  onChange={handleChange}
                  required
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="customerDateOfBirth">
                  Date Of Birth
                </Label>
                <Input
                  id="customerDateOfBirth"
                  type="date"
                  name="customerDateOfBirth"
                  value={formData.customerDateOfBirth}
                  onChange={handleChange}
                  required
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="customerNationalId">
                  National ID
                </Label>
                <Input
                  id="customerNationalId"
                  type="number"
                  name="customerNationalId"
                  value={formData.customerNationalId}
                  onChange={handleChange}
                  required
                />
              </div>

              <div className="space-y-2">
                <Label>Gender</Label>
                <Select
                  value={String(formData.isMale)}
                  onValueChange={(value) =>
                    setFormData((prev) => ({
                      ...prev,
                      isMale: value === "true",
                    }))
                  }
                >
                  <SelectTrigger>
                    <SelectValue />
                  </SelectTrigger>

                  <SelectContent>
                    <SelectItem value="true">
                      Male
                    </SelectItem>
                    <SelectItem value="false">
                      Female
                    </SelectItem>
                  </SelectContent>
                </Select>
              </div>

              <div className="space-y-2">
                <Label htmlFor="grade">Grade</Label>
                <Input
                  id="grade"
                  type="number"
                  name="grade"
                  value={formData.grade}
                  onChange={handleChange}
                  required
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="customerEmail">
                  Email Address
                </Label>
                <Input
                  id="customerEmail"
                  type="email"
                  name="customerEmail"
                  value={formData.customerEmail}
                  onChange={handleChange}
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="customerPhoenNumber">
                  Phone Number
                </Label>
                <Input
                  id="customerPhoenNumber"
                  name="customerPhoenNumber"
                  value={formData.customerPhoenNumber}
                  onChange={handleChange}
                  required
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="adress">Address</Label>
                <Input
                  id="adress"
                  name="adress"
                  value={formData.adress}
                  onChange={handleChange}
                  required
                />
              </div>
            </div>

            <div className="space-y-2">
              <Label htmlFor="notes">Notes</Label>
              <Textarea
                id="notes"
                name="notes"
                value={formData.notes}
                onChange={handleChange}
                rows={4}
              />
            </div>

            {message && (
              <div className="rounded-md border p-3 text-sm">
                {message}
              </div>
            )}

            <Button type="submit" className="w-full">
              Add Customer
            </Button>
          </form>
        </CardContent>
      </Card>
    </div>
  );
}